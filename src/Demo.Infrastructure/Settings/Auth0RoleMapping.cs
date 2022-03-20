using System;

namespace Demo.Infrastructure.Settings
{
    public class Auth0RoleMapping
    {
        public string Auth0RoleId { get; set; }
        public Guid InternalRoleId { get; set; }
        public string Name { get; set; }
    }
}
