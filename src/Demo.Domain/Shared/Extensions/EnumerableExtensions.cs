using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Shared.DomainEntity;

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