using Demo.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class AllowUnauthorizedAuthorizationHandler : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
