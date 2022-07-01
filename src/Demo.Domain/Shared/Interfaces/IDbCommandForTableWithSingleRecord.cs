using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IDbCommandForTableWithSingleRecord<T> : IDbCommand<T> where T : IEntity
    {
        Task<T> GetAsync(CancellationToken cancellationToken = default);
    }
}