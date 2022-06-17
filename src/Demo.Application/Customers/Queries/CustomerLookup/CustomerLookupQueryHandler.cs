using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Customers.Queries.CustomerLookup.Dtos;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Domain.Customer;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Customers.Queries.CustomerLookup
{
    public class CustomerLookupQueryHandler : IRequestHandler<CustomerLookupQuery, CustomerLookupQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<Customer> _query;

        public CustomerLookupQueryHandler(
            IDbQuery<Customer> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<CustomerLookupQueryResult> Handle(CustomerLookupQuery request,
            CancellationToken cancellationToken)
        {
            var query = request.Ids is { Length: > 0 }
                ? _query.WithOptions(x => x.IncludeDeleted = true).AsQueryable()
                : _query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var isValidInteger = int.TryParse(request.SearchTerm, NumberStyles.Integer,
                    CultureInfo.GetCultureInfo("nl-NL"), out var integerValue);

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, $"%{request.SearchTerm}%")
                    || (isValidInteger && EF.Functions.Like(x.Code.ToString(), $"%{request.SearchTerm}%"))
                );
            }

            if (request.Ids is { Length: > 0 })
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

            query = request.OrderBy switch
            {
                CustomerLookupOrderByEnum.Code => query.OrderBy(x => x.Code, sortOrder),
                CustomerLookupOrderByEnum.Name => query.OrderBy(x => x.Name, sortOrder),
                _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
            };

            var customers = await query
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ProjectTo<CustomerLookupDto>(_mapper.ConfigurationProvider)
                //.WriteQueryStringToOutputWindowIfInDebugMode()
                .ToListAsync(cancellationToken);

            return new CustomerLookupQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Customers = customers
            };
        }
    }
}