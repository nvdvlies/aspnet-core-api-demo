using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IUserPermissionsProvider
    {
        Task<List<Role.Permission>> GetAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Role.Permission>> GetAsync(Guid userId, bool refreshCache, CancellationToken cancellationToken);
    }
}