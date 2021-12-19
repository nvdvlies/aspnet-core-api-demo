using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.CopyInvoice
{
    public class CopyInvoiceCommandHandler : IRequestHandler<CopyInvoiceCommand, CopyInvoiceResponse>
    {
        private readonly IInvoiceDomainEntity _domainEntity;

        public CopyInvoiceCommandHandler(
            IInvoiceDomainEntity domainEntity
        )
        {
            _domainEntity = domainEntity;
        }

        public async Task<CopyInvoiceResponse> Handle(CopyInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsNewCopyAsync(request.Id, cancellationToken);

            await _domainEntity.CreateAsync(cancellationToken);

            return new CopyInvoiceResponse
            {
                Id = _domainEntity.EntityId
            };
        }
    }
}