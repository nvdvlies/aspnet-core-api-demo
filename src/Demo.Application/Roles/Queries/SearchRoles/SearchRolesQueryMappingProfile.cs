using AutoMapper;
using Demo.Application.Roles.Queries.SearchRoles.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Roles.Queries.SearchRoles
{
    public class SearchRolesQueryMappingProfile : Profile
    {
        public SearchRolesQueryMappingProfile()
        {
            CreateMap<Role, SearchRoleDto>();
        }
    }
}
