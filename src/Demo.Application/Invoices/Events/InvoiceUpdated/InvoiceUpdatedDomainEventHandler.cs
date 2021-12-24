using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceUpdated
{
    public class InvoiceUpdatedDomainEventHandler : INotificationHandler<InvoiceUpdatedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceUpdatedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceUpdatedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceUpdated(@event.Id, @event.UpdatedBy);
        }
    }
}
