using AutoMapper;
using Demo.Application.Users.Queries.UserLookup.Dtos;
using Demo.Domain.User;

namespace Demo.Application.Users.Queries.UserLookup
{
    public class UserLookupQueryMappingProfile : Profile
    {
        public UserLookupQueryMappingProfile()
        {
            CreateMap<User, UserLookupDto>();
        }
    }
}
