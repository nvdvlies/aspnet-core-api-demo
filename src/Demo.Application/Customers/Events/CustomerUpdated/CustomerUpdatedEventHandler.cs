using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Customer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Customers.Events.CustomerUpdated
{
    public class CustomerUpdatedEventHandler : INotificationHandler<CustomerUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<CustomerUpdatedEventHandler> _logger;

        public CustomerUpdatedEventHandler(
            ILogger<CustomerUpdatedEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(CustomerUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(CustomerUpdatedEvent)}");
            await _eventHubContext.All.CustomerUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}