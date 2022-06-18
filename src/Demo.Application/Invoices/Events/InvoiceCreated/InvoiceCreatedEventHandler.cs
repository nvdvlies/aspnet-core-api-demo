using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;

namespace Demo.Application.Invoices.Events.InvoiceCreated
{
    public class InvoiceCreatedEventHandler : INotificationHandler<InvoiceCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceCreatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}
