using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Customer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Customers.Events.CustomerCreated
{
    public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<CustomerCreatedEventHandler> _logger;

        public CustomerCreatedEventHandler(
            ILogger<CustomerCreatedEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(CustomerCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(CustomerCreatedEvent)}");
            return _eventHubContext.All.CustomerCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}
