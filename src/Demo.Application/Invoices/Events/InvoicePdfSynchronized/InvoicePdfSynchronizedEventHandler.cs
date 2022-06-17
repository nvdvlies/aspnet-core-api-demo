using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;

namespace Demo.Application.Invoices.Events.InvoicePdfSynchronized
{
    public class InvoicePdfSynchronizedEventHandler : INotificationHandler<InvoicePdfSynchronizedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoicePdfSynchronizedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoicePdfSynchronizedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoicePdfSynchronized(@event.Data.Id);
        }
    }
}