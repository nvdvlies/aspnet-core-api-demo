using System.Collections.Generic;
using Demo.Domain.Shared.Extensions;

namespace Demo.Domain.Shared.DomainEntity;

internal static class ValidationResult
{
    internal static IEnumerable<ValidationMessage> Ok()
    {
        return default;
    }

    internal static IEnumerable<ValidationMessage> Invalid(string message, string propertyName = null)
    {
        return message.ToValidationMessage(propertyName);
    }
}
