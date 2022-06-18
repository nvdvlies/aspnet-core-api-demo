using System;

namespace Demo.Domain.User
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; internal set; }

        public Guid RoleId { get; set; }
        public Role.Role Role { get; internal set; }
    }
}
