using Demo.Application.Shared.Interfaces;
using Demo.Domain.Customer.DomainEntity.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Events.CustomerCreated
{
    public class CustomerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public CustomerCreatedDomainEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.CustomerCreated(@event.Id, @event.CreatedBy);
        }
    }
}
