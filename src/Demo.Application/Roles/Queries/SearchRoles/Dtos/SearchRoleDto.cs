using Demo.Application.Shared.Dtos;

namespace Demo.Application.Roles.Queries.SearchRoles.Dtos
{
    public class SearchRoleDto : SoftDeleteEntityDto
    {
        public string Name { get; set; }
        public string ExternalId { get; set; }
    }
}