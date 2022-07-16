using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role;

namespace Demo.Domain.Shared.Interfaces;

public interface IPermissionGroupsProvider
{
    Task<List<PermissionGroup>> GetAsync(CancellationToken cancellationToken);
    Task<List<PermissionGroup>> GetAsync(bool refreshCache, CancellationToken cancellationToken);
}
