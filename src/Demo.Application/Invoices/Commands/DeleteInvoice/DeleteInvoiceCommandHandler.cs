using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Unit>
{
    private readonly IInvoiceDomainEntity _invoiceDomainEntity;

    public DeleteInvoiceCommandHandler(
        IInvoiceDomainEntity invoiceDomainEntity
    )
    {
        _invoiceDomainEntity = invoiceDomainEntity;
    }

    public async Task<Unit> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        await _invoiceDomainEntity.GetAsync(request.Id, cancellationToken);

        await _invoiceDomainEntity.DeleteAsync(cancellationToken);

        return Unit.Value;
    }
}
