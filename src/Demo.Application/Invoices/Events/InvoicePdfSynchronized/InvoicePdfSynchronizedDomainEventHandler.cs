using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoicePdfSynchronized
{
    public class InvoicePdfSynchronizedDomainEventHandler : INotificationHandler<InvoicePdfSynchronizedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoicePdfSynchronizedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoicePdfSynchronizedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoicePdfSynchronized(@event.Id);
        }
    }
}
