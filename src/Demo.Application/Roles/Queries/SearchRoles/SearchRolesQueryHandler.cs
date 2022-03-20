using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Application.Roles.Queries.SearchRoles.Dtos;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Roles.Queries.SearchRoles
{
    public class SearchRolesQueryHandler : IRequestHandler<SearchRolesQuery, SearchRolesQueryResult>
    {
        private readonly IDbQuery<Role> _query;
        private readonly IMapper _mapper;

        public SearchRolesQueryHandler(
            IDbQuery<Role> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<SearchRolesQueryResult> Handle(SearchRolesQuery request, CancellationToken cancellationToken)
        {
            var query = _query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Name, $"%{request.SearchTerm}%")
                );
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

            query = request.OrderBy switch
            {
                SearchRoleOrderByEnum.Name => query.OrderBy(x => x.Name, sortOrder),
                _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
            };

            var roles = await query
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ProjectTo<SearchRoleDto>(_mapper.ConfigurationProvider)
                //.WriteQueryStringToOutputWindowIfInDebugMode()
                .ToListAsync(cancellationToken);

            return new SearchRolesQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Roles = roles
            };
        }
    }
}