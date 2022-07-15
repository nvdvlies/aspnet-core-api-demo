using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.WebApi.Tests.Helpers
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string DefaultScheme = "Test";

        private readonly TestUser _testUser;

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TestUser testUser)
            : base(options, logger, encoder, clock)
        {
            _testUser = testUser;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!_testUser.IsAuthenticated)
            {
                return Task.FromResult(AuthenticateResult.Fail(""));
            }

            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _testUser.User.ExternalId) };
            var identity = new ClaimsIdentity(claims, DefaultScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, DefaultScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
