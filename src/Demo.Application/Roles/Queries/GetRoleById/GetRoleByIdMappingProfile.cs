using AutoMapper;
using Demo.Application.Roles.Queries.GetRoleById.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Roles.Queries.GetRoleById
{
    public partial class GetRoleByIdMappingProfile : Profile
    {
        public GetRoleByIdMappingProfile()
        {
            CreateMap<Role, RoleDto>();
        }
    }
}
