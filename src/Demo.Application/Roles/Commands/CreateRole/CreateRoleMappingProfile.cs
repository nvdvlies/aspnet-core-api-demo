using AutoMapper;
using Demo.Application.Roles.Commands.CreateRole.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Roles.Commands.CreateRole
{
    public class CreateRoleMappingProfile : Profile
    {
        public CreateRoleMappingProfile()
        {
            CreateMap<CreateRoleCommand, Role>()
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedBy, opt => opt.Ignore())
                .ForMember(x => x.DeletedOn, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.xmin, opt => opt.Ignore());

            CreateMap<CreateRoleCommandRolePermission, RolePermission>()
                .ForMember(x => x.RoleId, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore())
                .ForMember(x => x.Permission, opt => opt.Ignore());
        }
    }
}
