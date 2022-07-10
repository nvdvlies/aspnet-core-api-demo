using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Permissions
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string permissionName) : base(typeof(PermissionAuthorizationFilter))
        {
            Arguments = new object[] { new PermissionRequirement(permissionName) };
            Order = int.MinValue;
        }
    }
}