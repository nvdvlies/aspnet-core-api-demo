using AutoMapper;
using Demo.Application.Users.Queries.GetUserById.Dtos;
using Demo.Domain.User;

namespace Demo.Application.Users.Queries.GetUserById
{
    public partial class GetUserByIdMappingProfile : Profile
    {
        public GetUserByIdMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserRole, UserRoleDto>();
        }
    }
}
