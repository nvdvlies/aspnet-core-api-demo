using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }
        public byte[] Timestamp { get; set; }
        public string Name { get; set; }

        public void SetCustomerId(Guid id)
        {
            Id = id;
        }
    }
}