using System.Collections.Generic;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Role;

public class PermissionGroup : Entity, IQueryableEntity
{
    public string Name { get; set; }

    public List<Permission> Permissions { get; set; }
}
