using System;
using Demo.Application.Customers.Commands.CreateCustomer.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : ICommand, IRequest<CreateCustomerResponse>
{
    public string Name { get; set; }
    public Guid? AddressId { get; set; }
    public CreateCustomerLocationDto Address { get; set; }
}
