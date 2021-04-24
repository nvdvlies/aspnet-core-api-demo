using Demo.Domain.Shared.BusinessComponent;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IBeforeCreate<T> where T : IEntity
    {
        int Order => 0;
        Task ExecuteAsync(HookType type, IBusinessComponentContext<T> context, CancellationToken cancellationToken = default);
    }
}
