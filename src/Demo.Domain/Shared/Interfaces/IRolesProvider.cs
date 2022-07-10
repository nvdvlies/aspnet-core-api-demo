using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IRolesProvider
    {
        Task<List<Role.Role>> GetAsync();
        Task<List<Role.Role>> GetAsync(bool refreshCache);
    }
}