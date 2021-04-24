using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IDefaultValuesSetter<T> where T : IEntity
    {
        int Order => 0;
        Task SetDefaultValuesAsync(T entity, IBusinessComponentState state, CancellationToken cancellationToken = default);
    }
}
