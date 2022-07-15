using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserPermissions
{
    public class GetCurrentUserPermissionsQueryResult
    {
        public IEnumerable<PermissionDto> Permissions { get; set; }
    }
}
