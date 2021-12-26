using Demo.Application.Shared.Interfaces;
using Demo.Events.Customer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Events.CustomerCreated
{
    public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public CustomerCreatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.CustomerCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}
