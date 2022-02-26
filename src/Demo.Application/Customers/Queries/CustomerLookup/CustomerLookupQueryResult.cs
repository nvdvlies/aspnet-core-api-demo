using Demo.Application.Customers.Queries.CustomerLookup.Dtos;
using Demo.Application.Shared.Models;
using System.Collections.Generic;

namespace Demo.Application.Customers.Queries.CustomerLookup
{
    public class CustomerLookupQueryResult: BasePaginatedResult
    {
        public IEnumerable<CustomerLookupDto> Customers { get; set; }
    }
}