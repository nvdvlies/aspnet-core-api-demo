using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IPermissionGroupsProvider
    {
        Task<List<Role.PermissionGroup>> GetAsync(CancellationToken cancellationToken);
        Task<List<Role.PermissionGroup>> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}