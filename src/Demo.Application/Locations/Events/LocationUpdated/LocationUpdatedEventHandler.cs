using Demo.Application.Shared.Interfaces;
using Demo.Events.Location;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Locations.Events.LocationUpdated
{
    public class LocationUpdatedEventHandler : INotificationHandler<LocationUpdatedEvent>
    {
        private readonly ILogger<LocationUpdatedEventHandler> _logger;
        private readonly IEventHubContext _eventHubContext;

        public LocationUpdatedEventHandler(
            ILogger<LocationUpdatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(LocationUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(LocationUpdatedEvent)}");
            await _eventHubContext.All.LocationUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
