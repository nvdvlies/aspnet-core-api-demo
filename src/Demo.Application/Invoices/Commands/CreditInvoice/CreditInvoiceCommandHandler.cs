using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.CreditInvoice
{
    public class CreditInvoiceCommandHandler : IRequestHandler<CreditInvoiceCommand, CreditInvoiceResponse>
    {
        private readonly IInvoiceDomainEntity _domainEntity;

        public CreditInvoiceCommandHandler(
            IInvoiceDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<CreditInvoiceResponse> Handle(CreditInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsNewCreditAsync(request.Id, cancellationToken);

            await _domainEntity.CreateAsync(cancellationToken);

            return new CreditInvoiceResponse
            {
                Id = _domainEntity.EntityId
            };
        }
    }
}