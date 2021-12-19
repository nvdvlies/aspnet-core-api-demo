using Demo.Domain.Shared.DomainEntity;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.Shared.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static string AsString(this IEnumerable<ValidationMessage> validationMessages)
        {
            return string.Join(", ", validationMessages.Select(message => message.ToString()));
        }
    }
}
