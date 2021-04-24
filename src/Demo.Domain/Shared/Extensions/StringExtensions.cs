using Demo.Domain.Shared.BusinessComponent;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.Shared.Extensions
{
    internal static class StringExtensions
    {
        internal static IEnumerable<ValidationMessage> AsEnumerableOfValidationMessages(this string errorMessage)
        {
            var validationMessages = new[]
            {
                new ValidationMessage
                {
                    Message = errorMessage
                }
            };
            return validationMessages.AsEnumerable();
        }
    }
}
