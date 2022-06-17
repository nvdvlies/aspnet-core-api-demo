using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Customer;
using MediatR;

namespace Demo.Application.Customers.Events.CustomerDeleted
{
    public class CustomerDeletedEventHandler : INotificationHandler<CustomerDeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public CustomerDeletedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerDeletedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.CustomerDeleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}