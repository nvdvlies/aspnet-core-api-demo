using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.CreditInvoice;

public class CreditInvoiceCommandHandler : IRequestHandler<CreditInvoiceCommand, CreditInvoiceResponse>
{
    private readonly IInvoiceDomainEntity _invoiceDomainEntity;

    public CreditInvoiceCommandHandler(
        IInvoiceDomainEntity invoiceDomainEntity
    )
    {
        _invoiceDomainEntity = invoiceDomainEntity;
    }

    public async Task<CreditInvoiceResponse> Handle(CreditInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        await _invoiceDomainEntity.GetAsNewCreditAsync(request.Id, cancellationToken);

        await _invoiceDomainEntity.CreateAsync(cancellationToken);

        return new CreditInvoiceResponse { Id = _invoiceDomainEntity.EntityId };
    }
}
