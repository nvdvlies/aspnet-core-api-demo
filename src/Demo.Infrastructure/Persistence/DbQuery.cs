using System;
using System.Linq;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence
{
    internal class DbQuery<T> : IDbQuery<T> where T : class, IEntity
    {
        protected readonly IApplicationDbContext DbContext;
        protected readonly IDbQueryOptions Options;

        public DbQuery(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            Options = new DbQueryOptions();
        }

        public IDbQuery<T> WithOptions(Action<IDbQueryOptions> action)
        {
            action(Options);
            return this;
        }

        public IQueryable<T> AsQueryable()
        {
            return DbContext.Set<T>()
                .AsNoTracking()
                .AsQueryable();
        }
    }
}
