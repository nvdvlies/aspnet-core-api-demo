using Demo.Domain.Shared.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.DomainEntity
{
    internal static class Validations
    {
        internal static Task<IEnumerable<ValidationMessage>> Ok() => Task.FromResult<IEnumerable<ValidationMessage>>(default);
        internal static Task<IEnumerable<ValidationMessage>> Invalid(string message) => Task.FromResult(message.ToValidationMessage());
    }
}
