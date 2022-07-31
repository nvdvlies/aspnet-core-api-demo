using AutoMapper;
using Demo.Application.Locations.Queries.LocationLookup.Dtos;
using Demo.Domain.Location;

namespace Demo.Application.Locations.Queries.LocationLookup;

public class LocationLookupQueryMappingProfile : Profile
{
    public LocationLookupQueryMappingProfile()
    {
        CreateMap<Location, LocationLookupDto>();
    }
}
