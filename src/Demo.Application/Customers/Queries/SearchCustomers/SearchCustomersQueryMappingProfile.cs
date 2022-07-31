using AutoMapper;
using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Domain.Customer;

namespace Demo.Application.Customers.Queries.SearchCustomers;

public class SearchCustomersQueryMappingProfile : Profile
{
    public SearchCustomersQueryMappingProfile()
    {
        CreateMap<Customer, SearchCustomerDto>();
    }
}
