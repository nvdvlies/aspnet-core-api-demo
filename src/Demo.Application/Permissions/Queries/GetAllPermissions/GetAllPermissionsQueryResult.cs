using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.Permissions.Queries.GetAllPermissions
{
    public class GetAllPermissionsQueryResult
    {
        public IEnumerable<PermissionDto> Permissions { get; set; }
    }
}
