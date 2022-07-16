using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand : ICommand, IRequest<Unit>
{
    internal Guid Id { get; set; }

    // ReSharper disable once InconsistentNaming
    public uint xmin { get; set; }
    public string Name { get; set; }

    public void SetCustomerId(Guid id)
    {
        Id = id;
    }
}
