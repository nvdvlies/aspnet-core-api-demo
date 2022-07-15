using AutoMapper;
using Demo.Application.Roles.Queries.GetRoleById.Dtos;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Shared.Mappings
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionGroup, PermissionGroupDto>();
            CreateMap<RolePermission, RolePermissionDto>();
        }
    }
}
