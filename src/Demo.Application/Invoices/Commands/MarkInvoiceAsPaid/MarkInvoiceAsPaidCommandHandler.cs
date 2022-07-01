using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsPaid
{
    public class MarkInvoiceAsPaidCommandHandler : IRequestHandler<MarkInvoiceAsPaidCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _invoiceDomainEntity;

        public MarkInvoiceAsPaidCommandHandler(
            IInvoiceDomainEntity invoiceDomainEntity
        )
        {
            _invoiceDomainEntity = invoiceDomainEntity;
        }

        public async Task<Unit> Handle(MarkInvoiceAsPaidCommand request, CancellationToken cancellationToken)
        {
            await _invoiceDomainEntity.GetAsync(request.Id, cancellationToken);

            _invoiceDomainEntity.SetStatus(InvoiceStatus.Paid);

            await _invoiceDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}