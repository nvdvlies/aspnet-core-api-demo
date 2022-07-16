using System.Collections.Generic;
using Demo.Application.Customers.Queries.CustomerLookup.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Customers.Queries.CustomerLookup;

public class CustomerLookupQueryResult : BasePaginatedResult
{
    public IEnumerable<CustomerLookupDto> Customers { get; set; }
}
