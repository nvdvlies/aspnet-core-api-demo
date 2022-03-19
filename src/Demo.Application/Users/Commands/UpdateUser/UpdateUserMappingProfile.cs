using AutoMapper;
using Demo.Application.Users.Commands.UpdateUser.Dtos;
using Demo.Domain.User;

namespace Demo.Application.Users.Commands.UpdateUser
{
    public class UpdateUserMappingProfile : Profile
    {
        public UpdateUserMappingProfile()
        {
            CreateMap<UpdateUserCommand, User>()
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedBy, opt => opt.Ignore())
                .ForMember(x => x.DeletedOn, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Timestamp, opt => opt.Ignore());

            CreateMap<UpdateUserCommandUserRole, UserRole>()
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore());
        }
    }
}