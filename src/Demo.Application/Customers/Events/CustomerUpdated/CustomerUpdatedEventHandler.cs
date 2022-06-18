using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Customer;
using MediatR;

namespace Demo.Application.Customers.Events.CustomerUpdated
{
    public class CustomerUpdatedEventHandler : INotificationHandler<CustomerUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public CustomerUpdatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.CustomerUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
