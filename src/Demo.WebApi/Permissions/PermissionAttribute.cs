using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Permissions
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string permissionName) : base(typeof(PermissionAuthorizationFilterAttribute))
        {
            Arguments = new object[] { new PermissionRequirement(permissionName) };
            Order = int.MinValue;
        }
    }
}
