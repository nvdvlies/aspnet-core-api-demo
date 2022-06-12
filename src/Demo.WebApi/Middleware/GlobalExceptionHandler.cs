using Demo.Common.Interfaces;
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
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

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

                var includeDetailsInResponse = !env.IsProduction();

                var statusCode = HttpStatusCode.InternalServerError;
                var problemDetails = new ProblemDetails
                {
                    Status = (int)statusCode,
                    Title = exception.Message,
                    Detail = includeDetailsInResponse ? exception.ToString() : null,
                    Type = exception.GetType().Name
                };

                switch (exception)
                {
                    case DomainException domainException:
                        logger.LogInformation(exception, "A domain exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails = domainException.ToProblemDetails(statusCode, includeDetailsInResponse);
                        break;
                    case DomainValidationException domainValidationException:
                        logger.LogInformation(exception, "A domain validation exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails = domainValidationException.ToValidationProblemDetails(statusCode, includeDetailsInResponse);
                        break;
                    case DomainEntityNotFoundException domainEntityNotFoundException:
                        logger.LogInformation(exception, "A domain entity not found exception ocurred.");

                        statusCode = HttpStatusCode.NotFound;
                        problemDetails = domainEntityNotFoundException.ToProblemDetails(statusCode, includeDetailsInResponse);
                        break;
                    case ValidationException validationException:
                        logger.LogInformation(exception, "A validation exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails = validationException.ToValidationProblemDetails(statusCode, includeDetailsInResponse);
                        break;
                    case DbUpdateConcurrencyException dbUpdateConcurrencyException:
                        logger.LogWarning(exception, "A database update concurrency exception ocurred.");

                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails = dbUpdateConcurrencyException.ToProblemDetails(statusCode, includeDetailsInResponse);
                        break;
                    default:
                        logger.LogError(exception, "An unhandled exception occured.");

                        var correlationIdProvider = context.RequestServices.GetRequiredService<ICorrelationIdProvider>();
                        problemDetails.Extensions["traceId"] = correlationIdProvider.Id;
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
                WriteIndented = env.IsDevelopment() || env.IsDockerDev()
            };
            var json = JsonSerializer.Serialize(problemDetails, problemDetails.GetType(), options);
            await context.Response.WriteAsync(json);
        }
    }
}
