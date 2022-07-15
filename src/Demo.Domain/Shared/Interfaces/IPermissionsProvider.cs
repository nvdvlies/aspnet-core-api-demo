using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IPermissionsProvider
    {
        Task<List<Permission>> GetAsync(CancellationToken cancellationToken);
        Task<List<Permission>> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}
