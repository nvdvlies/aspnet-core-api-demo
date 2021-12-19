using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IBeforeDelete<T> where T : IEntity
    {
        int Order => 0;
        Task ExecuteAsync(HookType type, IDomainEntityContext<T> context, CancellationToken cancellationToken = default);
    }
}
