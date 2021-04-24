using Demo.Domain.Invoice;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsCancelled
{
    public class MarkInvoiceAsCancelledCommandHandler : IRequestHandler<MarkInvoiceAsCancelledCommand, Unit>
    {
        private readonly IInvoiceBusinessComponent _bc;

        public MarkInvoiceAsCancelledCommandHandler(
            IInvoiceBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<Unit> Handle(MarkInvoiceAsCancelledCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            _bc.SetStatus(InvoiceStatus.Cancelled);

            await _bc.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}