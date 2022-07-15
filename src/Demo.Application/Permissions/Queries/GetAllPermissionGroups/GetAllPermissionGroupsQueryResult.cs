using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.Permissions.Queries.GetAllPermissionGroups
{
    public class GetAllPermissionGroupsQueryResult
    {
        public IEnumerable<PermissionGroupDto> PermissionGroups { get; set; }
    }
}
