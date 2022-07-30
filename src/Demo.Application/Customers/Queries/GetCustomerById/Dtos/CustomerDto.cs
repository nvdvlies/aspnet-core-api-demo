using System;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.Customers.Queries.GetCustomerById.Dtos;

public class CustomerDto : SoftDeleteEntityDto
{
    public int Code { get; set; }
    public string Name { get; set; }

    public Guid? AddressId { get; set; }
    public LocationDto Address { get; set; }
}
