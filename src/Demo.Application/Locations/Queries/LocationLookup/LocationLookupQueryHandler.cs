using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Locations.Queries.LocationLookup.Dtos;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Domain.Location;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Locations.Queries.LocationLookup;

public class LocationLookupQueryHandler : IRequestHandler<LocationLookupQuery, LocationLookupQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IDbQuery<Location> _query;

    public LocationLookupQueryHandler(
        IDbQuery<Location> query,
        IMapper mapper
    )
    {
        _query = query;
        _mapper = mapper;
    }

    public async Task<LocationLookupQueryResult> Handle(LocationLookupQuery request, CancellationToken cancellationToken)
    {
        var query = _query.AsQueryable();

        if (request.Ids is { Length: > 0 })
        {
            query = query.Where(x => request.Ids.Contains(x.Id));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

        query = request.OrderBy switch
        {
            LocationLookupOrderByEnum.DisplayName => query.OrderBy(x => x.DisplayName, sortOrder),
            LocationLookupOrderByEnum.None => query,
            _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
        };

        var locations = await query
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ProjectTo<LocationLookupDto>(_mapper.ConfigurationProvider)
            //.WriteQueryStringToOutputWindowIfInDebugMode()
            .ToListAsync(cancellationToken);

        return new LocationLookupQueryResult
        {
            PageIndex = request.PageIndex, PageSize = request.PageSize, TotalItems = totalItems, Locations = locations
        };
    }
}
