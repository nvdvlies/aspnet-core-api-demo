using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence
{
    internal class DbCommandForTableWithSingleRecord<T> : DbCommand<T>, IDbCommandForTableWithSingleRecord<T> where T : class, IEntity
    {
        public DbCommandForTableWithSingleRecord(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<T> GetAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async override Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            var existing = await query.SingleOrDefaultAsync(cancellationToken);
            if (existing != null)
            {
                throw new Exception("Cannot insert item. Only one record is allowed in this recordset");
            }
            await base.InsertAsync(entity, cancellationToken);
        }
    }
}
