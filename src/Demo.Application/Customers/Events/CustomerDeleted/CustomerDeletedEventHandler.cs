using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Customer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Customers.Events.CustomerDeleted
{
    public class CustomerDeletedEventHandler : INotificationHandler<CustomerDeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<CustomerDeletedEventHandler> _logger;

        public CustomerDeletedEventHandler(
            ILogger<CustomerDeletedEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerDeletedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(CustomerDeletedEvent)}");
            await _eventHubContext.All.CustomerDeleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}