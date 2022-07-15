using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;

namespace Demo.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _dbContext;

        public UnitOfWork(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
