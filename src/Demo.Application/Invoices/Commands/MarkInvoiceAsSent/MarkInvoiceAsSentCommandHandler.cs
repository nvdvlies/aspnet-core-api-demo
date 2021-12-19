using Demo.Domain.Invoice;
using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsSent
{
    public class MarkInvoiceAsSentCommandHandler : IRequestHandler<MarkInvoiceAsSentCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _domainEntity;

        public MarkInvoiceAsSentCommandHandler(
            IInvoiceDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<Unit> Handle(MarkInvoiceAsSentCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            _domainEntity.SetStatus(InvoiceStatus.Sent);

            await _domainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}