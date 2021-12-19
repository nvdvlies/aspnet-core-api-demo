using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.DomainEntity.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceDeleted
{
    public class InvoiceDeletedDomainEventHandler : INotificationHandler<InvoiceDeletedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceDeletedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceDeleted(@event.Id, @event.DeletedBy);
        }
    }
}
