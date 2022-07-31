using System.Collections.Generic;
using Demo.Application.Locations.Queries.LocationLookup.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Locations.Queries.LocationLookup;

public class LocationLookupQueryResult : BasePaginatedResult
{
    public IEnumerable<LocationLookupDto> Locations { get; set; }
}
