using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Location;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Locations.Events.LocationUpdated;

public class LocationUpdatedEventHandler : INotificationHandler<LocationUpdatedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<LocationUpdatedEventHandler> _logger;

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
