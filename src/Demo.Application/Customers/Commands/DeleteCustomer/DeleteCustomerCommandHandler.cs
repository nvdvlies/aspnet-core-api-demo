using Demo.Domain.Customer.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ICustomerDomainEntity _domainEntity;

        public DeleteCustomerCommandHandler(
            ICustomerDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            await _domainEntity.DeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}