using System;

namespace Demo.Application.Customers.Queries.CustomerLookup.Dtos;

public class CustomerLookupDto
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
}
