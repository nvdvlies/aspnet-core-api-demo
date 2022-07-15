using System;
using System.Linq;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Seed;

namespace Demo.Domain.Role.Seed
{
    public static class AdministratorRole
    {
        public static readonly Guid RoleId = Guid.Parse("7C20005D-D5F8-4079-AF26-434D69B43C82");

        public static Role Create()
        {
            var user = new Role
            {
                Id = RoleId,
                Name = "Admin",
                ExternalId = "rol_N4KEnzIMUDaetcyr",
                RolePermissions = PermissionsSeed.All
                    .Select(permission => new RolePermission { RoleId = RoleId, PermissionId = permission.Id })
                    .ToList()
            };
            ((IAuditableEntity)user).SetCreatedByAndCreatedOn(DefaultAdministratorUser.UserId, DateTime.UtcNow);
            return user;
        }
    }
}
