using Demo.Domain.Shared.DomainEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal class BaseValidator {
        protected static readonly Task<IEnumerable<ValidationMessage>> CompletedTask = Task.FromResult<IEnumerable<ValidationMessage>>(default);
    }
}
