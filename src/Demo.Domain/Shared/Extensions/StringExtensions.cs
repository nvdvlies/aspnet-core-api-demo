using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Shared.DomainEntity;

namespace Demo.Domain.Shared.Extensions
{
    internal static class StringExtensions
    {
        internal static IEnumerable<ValidationMessage> ToValidationMessage(this string errorMessage,
            string propertyName = null)
        {
            var validationMessages = new[]
            {
                new ValidationMessage
                {
                    PropertyName = propertyName,
                    Message = errorMessage
                }
            };
            return validationMessages.AsEnumerable();
        }
    }
}