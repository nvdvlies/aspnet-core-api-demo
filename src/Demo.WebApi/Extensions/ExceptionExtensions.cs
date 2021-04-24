using Demo.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Demo.WebApi.Extensions
{
    public static class ExceptionExtensions
    {
        public static ProblemDetails ToProblemDetails(this DomainException domainException, HttpStatusCode statusCode, bool includeDetails = false)
        {
            return new ValidationProblemDetails
            {
                Status = (int)statusCode,
                Title = domainException.Message,
                Detail = includeDetails ? domainException.ToString() : null,
                Type = nameof(DomainValidationException)
            };
        }

        public static ValidationProblemDetails ToValidationProblemDetails(this DomainValidationException domainValidationException, HttpStatusCode statusCode, bool includeDetails = false)
        {
            var validationProblemDetails = new ValidationProblemDetails
            {
                Status = (int)statusCode,
                Title = domainValidationException.Message,
                Detail = includeDetails ? domainValidationException.ToString() : null,
                Type = nameof(DomainValidationException)
            };

            foreach (var validationMessage in domainValidationException.ValidationMessages)
            {
                validationProblemDetails.Errors.Add("_", new[] { validationMessage.Message });
            }

            return validationProblemDetails;
        }
    }
}
