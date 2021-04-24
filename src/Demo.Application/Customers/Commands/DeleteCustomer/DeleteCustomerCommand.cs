using MediatR;
using System;

namespace Demo.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetCustomerId(Guid id)
        {
            Id = id;
        }
    }
}