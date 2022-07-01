using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Roles.Queries.RoleLookup.Dtos;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Roles.Queries.RoleLookup
{
    public class RoleLookupQueryHandler : IRequestHandler<RoleLookupQuery, RoleLookupQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<Role> _query;

        public RoleLookupQueryHandler(
            IDbQuery<Role> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<RoleLookupQueryResult> Handle(RoleLookupQuery request, CancellationToken cancellationToken)
        {
            var query = request.Ids is { Length: > 0 }
                ? _query.WithOptions(x => x.IncludeDeleted = true).AsQueryable()
                : _query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{request.SearchTerm}%"));
            }

            if (request.Ids is { Length: > 0 })
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

            query = request.OrderBy switch
            {
                RoleLookupOrderByEnum.Name => query.OrderBy(x => x.Name, sortOrder),
                _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
            };

            var roles = await query
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ProjectTo<RoleLookupDto>(_mapper.ConfigurationProvider)
                //.WriteQueryStringToOutputWindowIfInDebugMode()
                .ToListAsync(cancellationToken);

            return new RoleLookupQueryResult
            {
                PageIndex = request.PageIndex, PageSize = request.PageSize, TotalItems = totalItems, Roles = roles
            };
        }
    }
}