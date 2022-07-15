using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Shared.DomainEntity;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
                        PropertyName = error.PropertyName, Message = error.ErrorMessage
                    });
                }
            }

            return validationMessages.AsEnumerable();
        }
    }
}
