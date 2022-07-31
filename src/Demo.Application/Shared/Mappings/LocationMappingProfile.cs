using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Location;

namespace Demo.Application.Shared.Mappings;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<Location, LocationDto>();
    }
}
