using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.BusinessComponent.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceCancelled
{
    public class InvoiceCancelledDomainEventHandler : INotificationHandler<InvoiceCancelledDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceCancelledDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceCancelledDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceCancelled(@event.Id);
        }
    }
}
