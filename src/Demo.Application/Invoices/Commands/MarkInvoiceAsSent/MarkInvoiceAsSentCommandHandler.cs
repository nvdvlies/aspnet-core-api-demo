using Demo.Domain.Invoice;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsSent
{
    public class MarkInvoiceAsSentCommandHandler : IRequestHandler<MarkInvoiceAsSentCommand, Unit>
    {
        private readonly IInvoiceBusinessComponent _bc;

        public MarkInvoiceAsSentCommandHandler(
            IInvoiceBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<Unit> Handle(MarkInvoiceAsSentCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            _bc.SetStatus(InvoiceStatus.Sent);

            await _bc.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}