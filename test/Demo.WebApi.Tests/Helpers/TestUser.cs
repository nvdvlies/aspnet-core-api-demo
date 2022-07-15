using Demo.Domain.User;

namespace Demo.WebApi.Tests.Helpers
{
    public class TestUser
    {
        public bool IsAuthenticated { get; set; }
        public User User { get; set; }
    }
}
