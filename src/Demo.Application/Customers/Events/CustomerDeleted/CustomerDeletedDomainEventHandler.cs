using Demo.Application.Shared.Interfaces;
using Demo.Domain.Customer.DomainEntity.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Events.CustomerDeleted
{
    public class CustomerDeletedDomainEventHandler : INotificationHandler<CustomerDeletedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public CustomerDeletedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.CustomerDeleted(@event.Id, @event.DeletedBy);
        }
    }
}
