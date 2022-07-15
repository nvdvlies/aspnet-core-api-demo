using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.Roles.Queries.GetRoleById.Dtos
{
    public class RoleDto : SoftDeleteEntityDto
    {
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public List<RolePermissionDto> RolePermissions { get; set; }
    }
}
