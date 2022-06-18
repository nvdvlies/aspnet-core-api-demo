using System.Net;
using Demo.Domain.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApi.Extensions
{
    public static class ExceptionExtensions
    {
        public static ProblemDetails ToProblemDetails(this DomainException exception, HttpStatusCode statusCode,
            bool includeDetails = false)
        {
            return new ProblemDetails
            {
                Status = (int)statusCode,
                Title = exception.Message,
                Detail = includeDetails ? exception.ToString() : null,
                Type = nameof(DomainException)
            };
        }

        public static ValidationProblemDetails ToValidationProblemDetails(this DomainValidationException exception,
            HttpStatusCode statusCode, bool includeDetails = false)
        {
            var validationProblemDetails = new ValidationProblemDetails
            {
                Status = (int)statusCode,
                Title = exception.Message,
                Detail = includeDetails ? exception.ToString() : null,
                Type = nameof(DomainValidationException)
            };

            foreach (var validationMessage in exception.ValidationMessages)
            {
                validationProblemDetails.Errors.Add(validationMessage.PropertyName ?? "_",
                    new[] { validationMessage.Message });
            }

            return validationProblemDetails;
        }

        public static ProblemDetails ToProblemDetails(this DomainEntityNotFoundException exception,
            HttpStatusCode statusCode, bool includeDetails = false)
        {
            return new ProblemDetails
            {
                Status = (int)statusCode,
                Title = exception.Message,
                Detail = includeDetails ? exception.ToString() : null,
                Type = nameof(DomainEntityNotFoundException)
            };
        }

        public static ValidationProblemDetails ToValidationProblemDetails(this ValidationException exception,
            HttpStatusCode statusCode, bool includeDetails = false)
        {
            var validationProblemDetails = new ValidationProblemDetails
            {
                Status = (int)statusCode,
                Title = exception.Message,
                Detail = includeDetails ? exception.ToString() : null,
                Type = nameof(ValidationException)
            };

            foreach (var error in exception.Errors)
            {
                validationProblemDetails.Errors.Add(error.PropertyName ?? "_", new[] { error.ErrorMessage });
            }

            return validationProblemDetails;
        }

        public static ProblemDetails ToProblemDetails(this DbUpdateConcurrencyException exception,
            HttpStatusCode statusCode, bool includeDetails = false)
        {
            return new ProblemDetails
            {
                Status = (int)statusCode,
                Title = exception.Message,
                Detail = includeDetails ? exception.ToString() : null,
                Type = nameof(DbUpdateConcurrencyException)
            };
        }
    }
}
