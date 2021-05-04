using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.CopyInvoice
{
    public class CopyInvoiceCommandHandler : IRequestHandler<CopyInvoiceCommand, CopyInvoiceResponse>
    {
        private readonly IInvoiceBusinessComponent _bc;

        public CopyInvoiceCommandHandler(
            IInvoiceBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<CopyInvoiceResponse> Handle(CopyInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetCopyAsync(request.Id, cancellationToken);

            await _bc.CreateAsync(cancellationToken);

            return new CopyInvoiceResponse
            {
                Id = _bc.EntityId
            };
        }
    }
}