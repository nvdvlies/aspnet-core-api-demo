using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IDbCommand<T> where T : IEntity
    {
        IDbCommand<T> WithOptions(Action<IDbCommandOptions> action);
        Task<T> GetAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellationToken = default);
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
