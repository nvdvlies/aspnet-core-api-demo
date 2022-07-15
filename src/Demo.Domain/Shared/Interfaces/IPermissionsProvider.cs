using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IPermissionsProvider
    {
        Task<List<Role.Permission>> GetAsync(CancellationToken cancellationToken);
        Task<List<Role.Permission>> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}