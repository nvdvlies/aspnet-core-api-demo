using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Unit>
    {
        private readonly IInvoiceBusinessComponent _bc;

        public DeleteInvoiceCommandHandler(
            IInvoiceBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<Unit> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            await _bc.DeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}