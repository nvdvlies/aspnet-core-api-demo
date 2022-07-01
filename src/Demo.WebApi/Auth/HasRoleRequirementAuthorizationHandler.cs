using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Demo.WebApi.Auth
{
    public class HasRoleRequirementAuthorizationHandler : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            HasRoleRequirement requirement)
        {
            var hasRolePermissionClaim = context.User.HasClaim(c =>
                c.Issuer == requirement.Issuer
                && c.Type == "permissions"
                && c.Value == requirement.Role
            );

            if (hasRolePermissionClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}