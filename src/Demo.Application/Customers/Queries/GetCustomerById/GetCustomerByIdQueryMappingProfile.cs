using AutoMapper;
using Demo.Application.Customers.Queries.GetCustomerById.Dtos;
using Demo.Domain.Customer;

namespace Demo.Application.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryMappingProfile : Profile
    {
        public GetCustomerByIdQueryMappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
