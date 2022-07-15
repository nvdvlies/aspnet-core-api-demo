using AutoMapper;
using Demo.Application.Roles.Commands.UpdateRole.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleMappingProfile : Profile
    {
        public UpdateRoleMappingProfile()
        {
            CreateMap<UpdateRoleCommand, Role>()
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedBy, opt => opt.Ignore())
                .ForMember(x => x.DeletedOn, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UpdateRoleCommandRolePermission, RolePermission>()
                .ForMember(x => x.RoleId, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore())
                .ForMember(x => x.Permission, opt => opt.Ignore());
        }
    }
}