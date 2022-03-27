using System.Collections.Generic;

namespace Demo.Infrastructure.Settings
{
    public class Auth0
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Connection { get; set; }
        public string RedirectUrl { get; set; }
        public List<Auth0RoleMapping> Auth0RoleMappings { get; set; }
    }
}
