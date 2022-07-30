using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Domain.Customer;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Customers.Queries.SearchCustomers;

public class SearchCustomersQueryHandler : IRequestHandler<SearchCustomersQuery, SearchCustomersQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IDbQuery<Customer> _query;

    public SearchCustomersQueryHandler(
        IDbQuery<Customer> query,
        IMapper mapper
    )
    {
        _query = query;
        _mapper = mapper;
    }

    public async Task<SearchCustomersQueryResult> Handle(SearchCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _query.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var isValidInteger = int.TryParse(request.SearchTerm, out _);

            query = query.Where(x =>
                EF.Functions.ILike(x.Name, $"%{request.SearchTerm}%")
                || (isValidInteger && EF.Functions.ILike(x.Code.ToString(), $"%{request.SearchTerm}%"))
            );
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

        query = request.OrderBy switch
        {
            SearchCustomersOrderByEnum.Code => query.OrderBy(x => x.Code, sortOrder),
            SearchCustomersOrderByEnum.Name => query.OrderBy(x => x.Name, sortOrder),
            _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
        };

        var customers = await query
            .Include(customer => customer.Address)
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ProjectTo<SearchCustomerDto>(_mapper.ConfigurationProvider)
            //.WriteQueryStringToOutputWindowIfInDebugMode()
            .ToListAsync(cancellationToken);

        return new SearchCustomersQueryResult
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            Customers = customers
        };
    }
}
