using Microsoft.AspNetCore.Authorization;

namespace Demo.WebApi.Permissions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }
}
