using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _domainEntity;

        public DeleteInvoiceCommandHandler(
            IInvoiceDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<Unit> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            await _domainEntity.DeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}