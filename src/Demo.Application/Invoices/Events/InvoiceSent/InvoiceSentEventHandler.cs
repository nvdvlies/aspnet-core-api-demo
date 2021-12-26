using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceSent
{
    public class InvoiceSentEventHandler : INotificationHandler<InvoiceSentEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceSentEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceSentEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceSent(@event.Data.Id);
        }
    }
}
