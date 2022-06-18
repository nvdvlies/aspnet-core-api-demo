using System.Collections.Generic;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;

namespace Demo.Domain.Role
{
    public class Role : SoftDeleteEntity, IQueryableEntity
    {
        public string Name { get; set; }

        public string ExternalId { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
