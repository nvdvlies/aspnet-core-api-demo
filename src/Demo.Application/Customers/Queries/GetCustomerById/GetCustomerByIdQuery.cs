using MediatR;
using System;

namespace Demo.Application.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<GetCustomerByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}