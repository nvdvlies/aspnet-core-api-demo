using MediatR;

namespace Demo.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CreateCustomerResponse>
    {
        public string Name { get; set; }
    }
}