using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IBeforeDelete<T> where T : IEntity
    {
        int Order => 0;
        Task ExecuteAsync(HookType type, IBusinessComponentContext<T> context, CancellationToken cancellationToken = default);
    }
}
