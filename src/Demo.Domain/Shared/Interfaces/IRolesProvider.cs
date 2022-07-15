using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IRolesProvider
    {
        Task<List<Role.Role>> GetAsync(CancellationToken cancellationToken);
        Task<List<Role.Role>> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}
