using AutoMapper;
using Demo.Domain.User;

namespace Demo.Application.CurrentUser.Commands.UpdateCurrentUser
{
    public class UpdateCurrentUserMappingProfile : Profile
    {
        public UpdateCurrentUserMappingProfile()
        {
            CreateMap<UpdateCurrentUserCommand, User>()
                .ForMember(x => x.ExternalId, opt => opt.Ignore())
                .ForMember(x => x.Email, opt => opt.Ignore())
                .ForMember(x => x.Gender, opt => opt.Ignore())
                .ForMember(x => x.BirthDate, opt => opt.Ignore())
                .ForMember(x => x.UserRoles, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedBy, opt => opt.Ignore())
                .ForMember(x => x.DeletedOn, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.xmin, opt => opt.Ignore());
        }
    }
}