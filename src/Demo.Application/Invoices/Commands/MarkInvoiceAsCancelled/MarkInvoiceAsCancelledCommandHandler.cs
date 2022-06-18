using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsCancelled
{
    public class MarkInvoiceAsCancelledCommandHandler : IRequestHandler<MarkInvoiceAsCancelledCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _invoiceDomainEntity;

        public MarkInvoiceAsCancelledCommandHandler(
            IInvoiceDomainEntity invoiceDomainEntity
        )
        {
            _invoiceDomainEntity = invoiceDomainEntity;
        }

        public async Task<Unit> Handle(MarkInvoiceAsCancelledCommand request, CancellationToken cancellationToken)
        {
            await _invoiceDomainEntity.GetAsync(request.Id, cancellationToken);

            _invoiceDomainEntity.SetStatus(InvoiceStatus.Cancelled);

            await _invoiceDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
