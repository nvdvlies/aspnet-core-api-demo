using Demo.Application.Shared.Interfaces;
using Demo.Domain.Customer.DomainEntity.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Events.CustomerUpdated
{
    public class CustomerUpdatedDomainEventHandler : INotificationHandler<CustomerUpdatedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public CustomerUpdatedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerUpdatedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.CustomerUpdated(@event.Id, @event.UpdatedBy);
        }
    }
}
