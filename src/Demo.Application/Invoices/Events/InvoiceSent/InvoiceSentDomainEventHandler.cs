using Demo.Application.Shared.Interfaces;
using Demo.Domain.Invoice.DomainEntity.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.InvoiceSent
{
    public class InvoiceSentDomainEventHandler : INotificationHandler<InvoiceSentDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceSentDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceSentDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceSent(@event.Id);
        }
    }
}
