using AutoMapper;
using Demo.Application.Locations.Queries.LocationLookup.Dtos;
using Demo.Domain.Location;
using Demo.Domain.Shared.Entities;

namespace Demo.Application.Locations.Queries.LocationLookup;

public class LocationLookupQueryMappingProfile : Profile
{
    public LocationLookupQueryMappingProfile()
    {
        CreateMap<Location, LocationLookupDto>();
    }
}
