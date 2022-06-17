using System.Threading;
using System.Threading.Tasks;
using Demo.Messages.Invoice;
using MediatR;

namespace Demo.Application.Invoices.Messages.SynchronizeInvoicePdf
{
    public class SynchronizeInvoicePdfMessageHandler : IRequestHandler<SynchronizeInvoicePdfMessage, Unit>
    {
        public async Task<Unit> Handle(SynchronizeInvoicePdfMessage request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return Unit.Value;
        }
    }
}