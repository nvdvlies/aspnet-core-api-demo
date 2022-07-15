using System;
using System.Collections.Generic;
using Demo.Domain.Role.Seed;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.User.Seed
{
    public static class DefaultAdministratorUser
    {
        public static readonly Guid UserId = Guid.Parse("08463267-7065-4631-9944-08DA09D992D6");
        public static readonly string ExternalId = "auth0|08463267-7065-4631-9944-08da09d992d6";

        public static User Create()
        {
            var user = new User
            {
                Id = UserId,
                ExternalId = ExternalId,
                FamilyName = "Administrator",
                Fullname = "Administrator",
                Email = "demo@demo.com",
                UserRoles = new List<UserRole> { new() { UserId = UserId, RoleId = AdministratorRole.RoleId } }
            };
            ((IAuditableEntity)user).SetCreatedByAndCreatedOn(UserId, DateTime.UtcNow);
            return user;
        }
    }
}
