using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Demo.Infrastructure.Persistence
{
    internal class DbCommand<T> : IDbCommand<T> where T : class, IEntity
    {
        protected readonly IApplicationDbContext DbContext;
        protected readonly IDbCommandOptions Options;

        public DbCommand(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            Options = new DbCommandOptions();
        }

        public IDbCommand<T> WithOptions(Action<IDbCommandOptions> action)
        {
            action(Options);
            return this;
        }

        public virtual async Task<T> GetAsync(Guid id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            CancellationToken cancellationToken = default)
        {
            var query = Options.AsNoTracking
                ? DbContext.Set<T>().AsNoTracking().AsQueryable()
                : DbContext.Set<T>().AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
            DbContext.Set<T>().Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            DbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            DbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}