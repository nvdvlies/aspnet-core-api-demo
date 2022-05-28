using System.Collections.Generic;

namespace Demo.Infrastructure.Settings
{
    public class Auth0
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string RedirectUrl { get; set; }
        public Auth0Management Auth0Management { get; set; } = new Auth0Management();
    }
}
