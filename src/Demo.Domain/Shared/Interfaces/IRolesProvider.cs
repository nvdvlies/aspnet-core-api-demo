using System.Collections.Generic;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IRolesProvider
    {
        List<Role.Role> Get();
        List<Role.Role> Get(bool refreshCache);
    }
}