using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Application.Roles.Queries.RoleLookup.Dtos;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Roles.Queries.RoleLookup
{
    public class RoleLookupQueryHandler : IRequestHandler<RoleLookupQuery, RoleLookupQueryResult>
    {
        private readonly IDbQuery<Role> _query;
        private readonly IMapper _mapper;

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
            var query = _query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{request.SearchTerm}%"));
            }

            if (request.Ids != null && request.Ids.Length > 0)
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
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Roles = roles
            };
        }
    }
}