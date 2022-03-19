using Demo.Domain.Shared.Extensions;
using System.Collections.Generic;

namespace Demo.Domain.Shared.DomainEntity
{
    internal static class ValidationResult
    {
        internal static IEnumerable<ValidationMessage> Ok() => default;
        internal static IEnumerable<ValidationMessage> Invalid(string message, string propertyName = null) => message.ToValidationMessage(propertyName);
    }
}
