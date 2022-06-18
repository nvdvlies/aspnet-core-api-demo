using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsSent
{
    public class MarkInvoiceAsSentCommandHandler : IRequestHandler<MarkInvoiceAsSentCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _invoiceDomainEntity;

        public MarkInvoiceAsSentCommandHandler(
            IInvoiceDomainEntity invoiceDomainEntity
        )
        {
            _invoiceDomainEntity = invoiceDomainEntity;
        }

        public async Task<Unit> Handle(MarkInvoiceAsSentCommand request, CancellationToken cancellationToken)
        {
            await _invoiceDomainEntity.GetAsync(request.Id, cancellationToken);

            _invoiceDomainEntity.SetStatus(InvoiceStatus.Sent);

            await _invoiceDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
