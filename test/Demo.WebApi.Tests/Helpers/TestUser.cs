using System.Collections.Generic;
using Demo.Domain.Role;
using Demo.Domain.User;

namespace Demo.WebApi.Tests.Helpers
{
    public class TestUser
    {
        public bool IsAuthenticated { get; set; }
        public List<Role> Roles { get; set; }
        public User User { get; set; }
    }
}
