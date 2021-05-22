using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.BusinessComponent.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoicePaid
{
    public class InvoicePaidDomainEventHandler : INotificationHandler<InvoicePaidDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoicePaidDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoicePaidDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoicePaid(@event.Id);
        }
    }
}
