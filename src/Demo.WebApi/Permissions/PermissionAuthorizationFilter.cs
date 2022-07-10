using System;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.WebApi.Permissions
{
    public class PermissionAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly PermissionRequirement _permissionRequirement;

        public PermissionAuthorizationFilter(
            IPermissionChecker permissionChecker,
            PermissionRequirement permissionRequirement
        )
        {
            _permissionChecker = permissionChecker;
            _permissionRequirement = permissionRequirement;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var hasPermission = await _permissionChecker.HasPermissionAsync(_permissionRequirement.PermissionName);
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}