using AutoMapper;
using Demo.Application.Users.Queries.SearchUsers.Dtos;
using Demo.Domain.User;

namespace Demo.Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryMappingProfile : Profile
    {
        public SearchUsersQueryMappingProfile()
        {
            CreateMap<User, SearchUserDto>();
        }
    }
}