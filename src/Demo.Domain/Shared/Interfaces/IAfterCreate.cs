using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IAfterCreate<T> where T : IEntity
    {
        int Order => 0;

        Task ExecuteAsync(HookType type, IDomainEntityContext<T> context,
            CancellationToken cancellationToken = default);
    }
}
