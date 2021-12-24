using Demo.Domain.Shared.DomainEntity;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.Shared.Extensions
{
    internal static class ValidationResultExtensions
    {
        internal static IEnumerable<ValidationMessage> ToValidationMessage(this ValidationResult result)
        {
            var validationMessages = new List<ValidationMessage>();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    validationMessages.Add(new ValidationMessage
                    {
                        PropertyName = error.PropertyName,
                        Message = error.ErrorMessage
                    });
                }
            }
            return validationMessages.AsEnumerable();
        }
    }
}
