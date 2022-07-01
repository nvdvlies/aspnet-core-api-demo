using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IValidator<T> where T : IEntity
    {
        Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<T> context,
            CancellationToken cancellationToken = default);
    }
}