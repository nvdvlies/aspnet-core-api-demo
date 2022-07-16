using System.Collections.Generic;
using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Customers.Queries.SearchCustomers;

public class SearchCustomersQueryResult : BasePaginatedResult
{
    public IEnumerable<SearchCustomerDto> Customers { get; set; }
}
