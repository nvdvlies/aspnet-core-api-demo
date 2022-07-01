using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.DomainEntity
{
    internal static class ValidationResultTask
    {
        internal static Task<IEnumerable<ValidationMessage>> Ok()
        {
            return Task.FromResult(ValidationResult.Ok());
        }

        internal static Task<IEnumerable<ValidationMessage>> Invalid(string message, string propertyName = null)
        {
            return Task.FromResult(ValidationResult.Invalid(message, propertyName));
        }
    }
}