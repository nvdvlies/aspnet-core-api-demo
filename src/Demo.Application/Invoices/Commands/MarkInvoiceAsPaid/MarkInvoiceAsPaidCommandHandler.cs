using Demo.Domain.Invoice;
using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsPaid
{
    public class MarkInvoiceAsPaidCommandHandler : IRequestHandler<MarkInvoiceAsPaidCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _domainEntity;

        public MarkInvoiceAsPaidCommandHandler(
            IInvoiceDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<Unit> Handle(MarkInvoiceAsPaidCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            _domainEntity.SetStatus(InvoiceStatus.Paid);

            await _domainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}