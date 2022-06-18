using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;

namespace Demo.Application.Invoices.Events.InvoiceCancelled
{
    public class InvoiceCancelledEventHandler : INotificationHandler<InvoiceCancelledEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceCancelledEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceCancelledEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceCancelled(@event.Data.Id);
        }
    }
}
