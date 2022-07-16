using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.CopyInvoice;

public class CopyInvoiceCommandHandler : IRequestHandler<CopyInvoiceCommand, CopyInvoiceResponse>
{
    private readonly IInvoiceDomainEntity _invoiceDomainEntity;

    public CopyInvoiceCommandHandler(
        IInvoiceDomainEntity invoiceDomainEntity
    )
    {
        _invoiceDomainEntity = invoiceDomainEntity;
    }

    public async Task<CopyInvoiceResponse> Handle(CopyInvoiceCommand request, CancellationToken cancellationToken)
    {
        await _invoiceDomainEntity.GetAsNewCopyAsync(request.Id, cancellationToken);

        await _invoiceDomainEntity.CreateAsync(cancellationToken);

        return new CopyInvoiceResponse { Id = _invoiceDomainEntity.EntityId };
    }
}
