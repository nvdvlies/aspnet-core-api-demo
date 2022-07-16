using System;
using System.Collections.Generic;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Role;

public class Permission : Entity, IQueryableEntity
{
    public string Name { get; set; }
    public Guid? PermissionGroupId { get; set; }
    public PermissionGroup PermissionGroup { get; set; }
    public List<RolePermission> RolePermissions { get; set; }
}
