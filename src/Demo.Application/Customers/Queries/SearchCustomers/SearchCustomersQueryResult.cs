using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Application.Shared.Models;
using System.Collections.Generic;

namespace Demo.Application.Customers.Queries.SearchCustomers
{
    public class SearchCustomersQueryResult : BasePaginatedResult
    {
        public IEnumerable<SearchCustomerDto> Customers { get; set; }
    }
}