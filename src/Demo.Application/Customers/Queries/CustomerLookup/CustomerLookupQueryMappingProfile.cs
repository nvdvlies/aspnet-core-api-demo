using AutoMapper;
using Demo.Application.Customers.Queries.CustomerLookup.Dtos;
using Demo.Domain.Customer;

namespace Demo.Application.Customers.Queries.CustomerLookup;

public class CustomerLookupQueryMappingProfile : Profile
{
    public CustomerLookupQueryMappingProfile()
    {
        CreateMap<Customer, CustomerLookupDto>();
    }
}
