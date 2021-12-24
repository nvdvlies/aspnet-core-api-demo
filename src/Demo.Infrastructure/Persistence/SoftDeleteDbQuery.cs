using Demo.Domain.Shared.Interfaces;
using System.Linq;

namespace Demo.Infrastructure.Persistence
{
    internal class SoftDeleteDbQuery<T> : DbQuery<T>, IDbQuery<T> where T : class, IEntity, ISoftDeleteEntity
    {
        public SoftDeleteDbQuery(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public new IQueryable<T> AsQueryable()
        {
            var query = base.AsQueryable();

            if (!Options.IncludeDeleted)
            {
                query = query.Where(x => !x.Deleted);
            }

            return query;
        }
    }
}
