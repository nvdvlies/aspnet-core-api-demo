using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.CreditInvoice
{
    public class CreditInvoiceCommandHandler : IRequestHandler<CreditInvoiceCommand, CreditInvoiceResponse>
    {
        private readonly IInvoiceBusinessComponent _bc;

        public CreditInvoiceCommandHandler(
            IInvoiceBusinessComponent bc
        )
        {
            _bc = bc;
        }

        public async Task<CreditInvoiceResponse> Handle(CreditInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsCreditAsync(request.Id, cancellationToken);

            await _bc.CreateAsync(cancellationToken);

            return new CreditInvoiceResponse
            {
                Id = _bc.EntityId
            };
        }
    }
}