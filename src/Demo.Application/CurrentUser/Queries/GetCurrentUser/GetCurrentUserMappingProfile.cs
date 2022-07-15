using AutoMapper;
using Demo.Application.CurrentUser.Queries.GetCurrentUser.Dtos;
using Demo.Application.Users.Queries.GetUserById.Dtos;
using Demo.Domain.User;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUser
{
    public class GetCurrentUserMappingProfile : Profile
    {
        public GetCurrentUserMappingProfile()
        {
            CreateMap<User, CurrentUserDto>()
                .ForMember(dto => dto.Gender, opt => opt.MapFrom(entity => (GenderEnum)entity.Gender));
        }
    }
}
