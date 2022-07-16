using System;
using System.Collections.Generic;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Seed;

namespace Demo.Domain.Role.Seed;

public static class UserRole
{
    public static readonly Guid RoleId = Guid.Parse("D8A81CD5-D828-47AC-9F72-2E660F43A176");

    public static Role Create()
    {
        var user = new Role
        {
            Id = RoleId,
            Name = "User",
            ExternalId = "rol_OUaEQHOTuugOJHwe",
            RolePermissions = new List<RolePermission>
            {
                new() { RoleId = RoleId, PermissionId = PermissionsSeed.CustomersRead.Id },
                new() { RoleId = RoleId, PermissionId = PermissionsSeed.CustomersWrite.Id },
                new() { RoleId = RoleId, PermissionId = PermissionsSeed.InvoicesRead.Id },
                new() { RoleId = RoleId, PermissionId = PermissionsSeed.InvoicesWrite.Id }
            }
        };
        ((IAuditableEntity)user).SetCreatedByAndCreatedOn(DefaultAdministratorUser.UserId, DateTime.UtcNow);
        return user;
    }
}
