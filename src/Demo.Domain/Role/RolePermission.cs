using System;

namespace Demo.Domain.Role;

public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role Role { get; internal set; }

    public Guid PermissionId { get; set; }
    public Permission Permission { get; internal set; }
}
