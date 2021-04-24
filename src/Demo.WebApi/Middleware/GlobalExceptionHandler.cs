using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Demo.Domain.Shared.Exceptions;
using Demo.WebApi.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.WebApi.Middleware
{
    public class GlobalExceptionHandler
    {
        public static RequestDelegate Handle(IWebHostEnvironment env)
        {
            return async context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<GlobalExceptionHandler>>();

                var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

                var statusCode = HttpStatusCode.InternalServerError;
                var problemDetails = new ProblemDetails
                {
                    Status = (int)statusCode,
                    Title = exception.Message,
                    Detail = env.IsDevelopment() ? exception.ToString() : null,
                    Type = exception.GetType().Name ?? string.Empty
                };

                switch (exception)
                {
                    case DomainException domainException:
                        logger.LogInformation(exception, "A domain exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails = domainException.ToProblemDetails(statusCode, includeDetails: env.IsDevelopment());
                        break;
                    case DomainValidationException domainValidationException:
                        logger.LogInformation(exception, "A domain validation exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails = domainValidationException.ToValidationProblemDetails(statusCode, includeDetails: env.IsDevelopment());
                        break;
                    case DomainEntityNotFoundException domainEntityNotFoundException:
                        logger.LogInformation(exception, "A domain entity not found exception ocurred.");

                        statusCode = HttpStatusCode.NotFound;
                        //problemDetails = domainEntityNotFoundException.ToProblemDetails(statusCode, includeDetails: env.IsDevelopment());
                        break;
                    case ValidationException validationException:
                        logger.LogInformation(exception, "A validation exception ocurred.");

                        //problemDetails = validationException.ToValidationProblemDetails(statusCode, includeDetails: env.IsDevelopment());
                        break;
                    case DbUpdateConcurrencyException dbUpdateConcurrencyException:
                        logger.LogInformation(exception, "A database update concurrency exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        //problemDetails = exception.ToProblemDetails(statusCode, includeDetails: env.IsDevelopment());
                        break;
                    default:
                        logger.LogError(exception, "An unhandled exception occured.");
                        problemDetails.Extensions["traceId"] = Guid.NewGuid();
                        break;
                }

                await WriteResponseAsync(env, context, problemDetails, statusCode);
            };
        }

        private static async Task WriteResponseAsync(IWebHostEnvironment env, HttpContext context, ProblemDetails problemDetails, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/problem+json";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = env.IsDevelopment()
            };
            var json = JsonSerializer.Serialize(problemDetails, options);
            await context.Response.WriteAsync(json);
        }
    }

    //public class ExceptionHandlerMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public ExceptionHandlerMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
    //    {
    //        try
    //        {
    //            await _next(context);
    //        }
    //        catch (Exception exception)
    //        {
    //            if (exception is DomainException domainException)
    //            {
    //                logger.LogInformation(exception, "A domain exception ocurred.");

    //                var dto = new ValidationExceptionDto
    //                {
    //                    Errors = new List<ValidationFailureDto> {
    //                            new ValidationFailureDto {
    //                                ErrorMessage = domainException.Message
    //                            }
    //                        }
    //                };

    //                await WriteResponseAsync(context, (int)HttpStatusCode.BadRequest, dto);
    //            }
    //            else if (exception is DomainValidationException domainValidationException)
    //            {
    //                logger.LogInformation(exception, "A domain validation exception ocurred.");

    //                var dto = new ValidationExceptionDto
    //                {
    //                    Errors = domainValidationException
    //                        .ValidationMessages
    //                        .Select(x => new ValidationFailureDto
    //                        {
    //                            ErrorMessage = x.Message
    //                        })
    //                        .ToList()
    //                };

    //                await WriteResponseAsync(context, (int)HttpStatusCode.BadRequest, dto);
    //            }
    //            else if (exception is DomainEntityNotFoundException domainEntityNotFoundException)
    //            {
    //                logger.LogInformation(exception, "A domain entity not found exception ocurred.");

    //                await WriteResponseAsync(context, (int)HttpStatusCode.NotFound, null);
    //            }
    //            else if (exception is ValidationException validationException)
    //            {
    //                logger.LogInformation(exception, "A validation exception ocurred.");

    //                var dto = new ValidationExceptionDto
    //                {
    //                    Errors = validationException
    //                        .Errors
    //                        .Select(x => new ValidationFailureDto
    //                        {
    //                            PropertyName = x.PropertyName,
    //                            ErrorMessage = x.ErrorMessage
    //                        })
    //                        .ToList()
    //                };

    //                await WriteResponseAsync(context, (int)HttpStatusCode.BadRequest, dto);
    //            } 
    //            else if (exception is DbUpdateConcurrencyException concurrencyException)
    //            {
    //                logger.LogInformation(exception, "A concurrency exception ocurred.");

    //                var dto = new ValidationExceptionDto
    //                {
    //                    Errors = new List<ValidationFailureDto> {
    //                            new ValidationFailureDto {
    //                                ErrorMessage = "Het record is zojuist door een ander aangepast. Opslaan is daardoor niet mogelijk. Herlaad het scherm om de laatste gegevens op te halen. Uw wijzigingen gaan daarmee verloren."
    //                            }
    //                        }
    //                };

    //                await WriteResponseAsync(context, (int)HttpStatusCode.BadRequest, dto);
    //            }
    //            else
    //            {
    //                logger.LogError(exception, "An unhandled exception occured.");

    //                var operationId = Guid.NewGuid().ToString(); // context.Features.Get<RequestTelemetry>().Context.Operation.Id;

    //                var dto = new UnhandledExceptionDto
    //                {
    //                    OperationId = operationId,
    //                    Message = $"Sorry! Er ging iets mis. Probeer het later nogmaals. Blijft het probleem zich voordoen? Neem dan contact op met onze klantenservice. Incidentcode: {operationId}"
    //                };

    //                await WriteResponseAsync(context, (int)HttpStatusCode.InternalServerError, dto);
    //            }
    //        }
    //    }

    //    private async Task WriteResponseAsync(HttpContext context, int statusCode, object dto)
    //    {
    //        context.Response.StatusCode = statusCode;
    //        context.Response.ContentType = "application/json";
    //        if (dto != null)
    //        {
    //            var options = new JsonSerializerOptions
    //            {
    //                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //            };
    //            var json = JsonSerializer.Serialize(dto, options);
    //            await context.Response.WriteAsync(json);
    //        }
    //    }

    //}
}
