using AutoMapper;
using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Domain.Customer;
using Demo.Domain.Location;
using Demo.Domain.Shared.Entities;

namespace Demo.Application.Customers.Queries.SearchCustomers;

public class SearchCustomersQueryMappingProfile : Profile
{
    public SearchCustomersQueryMappingProfile()
    {
        CreateMap<Customer, SearchCustomerDto>();
    }
}
