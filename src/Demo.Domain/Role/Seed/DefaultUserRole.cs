using System;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Seed;

namespace Demo.Domain.Role.Seed
{
    public static class DefaultUserRole
    {
        public static readonly Guid RoleId = Guid.Parse("D8A81CD5-D828-47AC-9F72-2E660F43A176");

        public static Role Create()
        {
            var user = new Role
            {
                Id = RoleId, Name = "User", ExternalId = "rol_OUaEQHOTuugOJHwe"
            };
            ((IAuditableEntity)user).SetCreatedByAndCreatedOn(DefaultAdministratorUser.UserId, DateTime.UtcNow);
            return user;
        }
    }
}