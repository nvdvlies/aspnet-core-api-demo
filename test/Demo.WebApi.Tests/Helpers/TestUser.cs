using System.Collections.Generic;
using Demo.Domain.Role;

namespace Demo.WebApi.Tests.Helpers
{
    public class TestUser
    {
        public bool IsAuthenticated { get; set; }
        public List<Role> Roles { get; set; }
        public Domain.User.User User { get; set; }
    }
}