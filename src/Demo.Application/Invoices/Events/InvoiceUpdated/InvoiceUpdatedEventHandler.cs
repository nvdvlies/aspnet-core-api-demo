using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceUpdated
{
    public class InvoiceUpdatedEventHandler : INotificationHandler<InvoiceUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceUpdatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
