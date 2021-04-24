using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces
{
    public interface IUnitOfWork {
        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}