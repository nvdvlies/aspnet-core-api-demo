using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Location;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Locations.Events.LocationCreated;

public class LocationCreatedEventHandler : INotificationHandler<LocationCreatedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<LocationCreatedEventHandler> _logger;

    public LocationCreatedEventHandler(
        ILogger<LocationCreatedEventHandler> logger,
        IEventHubContext eventHubContext
    )
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
    }

    public async Task Handle(LocationCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(LocationCreatedEvent)}");
        await _eventHubContext.All.LocationCreated(@event.Data.Id, @event.Data.CreatedBy);
    }
}
