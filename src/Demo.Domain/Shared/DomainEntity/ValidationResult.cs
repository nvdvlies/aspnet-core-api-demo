using Demo.Domain.Shared.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.DomainEntity
{
    internal static class ValidationResult
    {
        internal static Task<IEnumerable<ValidationMessage>> Ok() => Task.FromResult<IEnumerable<ValidationMessage>>(default);
        internal static Task<IEnumerable<ValidationMessage>> Invalid(string message, string propertyName = null) => Task.FromResult(message.ToValidationMessage(propertyName));
    }
}
