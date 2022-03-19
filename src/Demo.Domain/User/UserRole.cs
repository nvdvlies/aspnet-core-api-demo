using System;

namespace Demo.Domain.User
{
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role.Role Role { get; set; }
    }
}