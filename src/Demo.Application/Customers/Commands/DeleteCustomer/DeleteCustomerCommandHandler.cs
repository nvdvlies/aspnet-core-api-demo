using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Customer.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ICustomerDomainEntity _customerDomainEntity;

        public DeleteCustomerCommandHandler(
            ICustomerDomainEntity customerDomainEntity
        )
        {
            _customerDomainEntity = customerDomainEntity;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            await _customerDomainEntity.GetAsync(request.Id, cancellationToken);

            await _customerDomainEntity.DeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
