using Demo.Application.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _dbContext;

        public UnitOfWork(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
