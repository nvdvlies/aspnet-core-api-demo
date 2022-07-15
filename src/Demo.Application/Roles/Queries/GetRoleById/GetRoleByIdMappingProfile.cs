using AutoMapper;
using Demo.Application.Roles.Queries.GetRoleById.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdMappingProfile : Profile
    {
        public GetRoleByIdMappingProfile()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RolePermission, RolePermissionDto>();
        }
    }
}
