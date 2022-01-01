using Demo.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class AcceptAllHasScopeRequirementAuthorizationHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
