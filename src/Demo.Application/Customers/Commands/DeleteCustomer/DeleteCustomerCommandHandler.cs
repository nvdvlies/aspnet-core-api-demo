using Demo.Domain.Customer.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ICustomerBusinessComponent _bc;

        public DeleteCustomerCommandHandler(
            ICustomerBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            await _bc.DeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}