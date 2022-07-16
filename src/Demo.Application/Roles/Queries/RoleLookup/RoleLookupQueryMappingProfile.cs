using AutoMapper;
using Demo.Application.Roles.Queries.RoleLookup.Dtos;
using Demo.Domain.Role;

namespace Demo.Application.Roles.Queries.RoleLookup;

public class RoleLookupQueryMappingProfile : Profile
{
    public RoleLookupQueryMappingProfile()
    {
        CreateMap<Role, RoleLookupDto>();
    }
}
