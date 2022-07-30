using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Location;
using Demo.Domain.Shared.Entities;

namespace Demo.Application.Shared.Mappings;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<Location, LocationDto>();
    }
}
