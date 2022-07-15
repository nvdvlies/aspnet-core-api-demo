using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IUserPermissionsProvider
    {
        Task<List<Permission>> GetAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Permission>> GetAsync(Guid userId, bool refreshCache, CancellationToken cancellationToken);
    }
}
