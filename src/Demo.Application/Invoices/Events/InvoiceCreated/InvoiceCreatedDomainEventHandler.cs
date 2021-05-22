using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.BusinessComponent.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceCreated
{
    public class InvoiceCreatedDomainEventHandler : INotificationHandler<InvoiceCreatedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceCreatedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceCreated(@event.Id, @event.CreatedBy);
        }
    }
}
