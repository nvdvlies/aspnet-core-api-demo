using Demo.Domain.Invoice;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsPaid
{
    public class MarkInvoiceAsPaidCommandHandler : IRequestHandler<MarkInvoiceAsPaidCommand, Unit>
    {
        private readonly IInvoiceBusinessComponent _bc;

        public MarkInvoiceAsPaidCommandHandler(
            IInvoiceBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<Unit> Handle(MarkInvoiceAsPaidCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            _bc.SetStatus(InvoiceStatus.Paid);

            await _bc.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}