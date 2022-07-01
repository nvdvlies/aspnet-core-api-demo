using AutoMapper;
using Demo.Application.Users.Queries.GetUserById.Dtos;
using Demo.Domain.User;

namespace Demo.Application.Users.Queries.GetUserById
{
    public class GetUserByIdMappingProfile : Profile
    {
        public GetUserByIdMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Gender, opt => opt.MapFrom(entity => (GenderEnum)entity.Gender));
            CreateMap<UserRole, UserRoleDto>();
        }
    }
}