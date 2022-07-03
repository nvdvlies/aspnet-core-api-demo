using System;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Seed;

namespace Demo.Domain.Role.Seed
{
    public static class DefaultAdministratorRole
    {
        public static readonly Guid RoleId = Guid.Parse("7C20005D-D5F8-4079-AF26-434D69B43C82");

        public static Role Create()
        {
            var user = new Role
            {
                Id = RoleId, Name = "Admin", ExternalId = "rol_N4KEnzIMUDaetcyr"
            };
            ((IAuditableEntity)user).SetCreatedByAndCreatedOn(DefaultAdministratorUser.UserId, DateTime.UtcNow);
            return user;
        }
    }
}