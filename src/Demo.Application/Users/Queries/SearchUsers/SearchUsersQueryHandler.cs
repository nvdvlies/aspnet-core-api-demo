using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Application.Users.Queries.SearchUsers.Dtos;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, SearchUsersQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<User> _query;

        public SearchUsersQueryHandler(
            IDbQuery<User> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<SearchUsersQueryResult> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Fullname, $"%{request.SearchTerm}%")
                    || EF.Functions.Like(x.Email, $"%{request.SearchTerm}%")
                );
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

            query = request.OrderBy switch
            {
                SearchUserOrderByEnum.Fullname => query.OrderBy(x => x.Fullname, sortOrder),
                SearchUserOrderByEnum.FamilyName => query.OrderBy(x => x.FamilyName, sortOrder),
                SearchUserOrderByEnum.Email => query.OrderBy(x => x.Email, sortOrder),
                _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
            };

            var users = await query
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ProjectTo<SearchUserDto>(_mapper.ConfigurationProvider)
                //.WriteQueryStringToOutputWindowIfInDebugMode()
                .ToListAsync(cancellationToken);

            return new SearchUsersQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Users = users
            };
        }
    }
}