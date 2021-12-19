using Demo.Domain.Invoice;
using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsCancelled
{
    public class MarkInvoiceAsCancelledCommandHandler : IRequestHandler<MarkInvoiceAsCancelledCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _domainEntity;

        public MarkInvoiceAsCancelledCommandHandler(
            IInvoiceDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<Unit> Handle(MarkInvoiceAsCancelledCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            _domainEntity.SetStatus(InvoiceStatus.Cancelled);

            await _domainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}