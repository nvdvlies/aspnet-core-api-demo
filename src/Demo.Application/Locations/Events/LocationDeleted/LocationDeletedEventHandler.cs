using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Location;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Locations.Events.LocationDeleted;

public class LocationDeletedEventHandler : INotificationHandler<LocationDeletedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<LocationDeletedEventHandler> _logger;

    public LocationDeletedEventHandler(
        ILogger<LocationDeletedEventHandler> logger,
        IEventHubContext eventHubContext
    )
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
    }

    public async Task Handle(LocationDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(LocationDeletedEvent)}");
        await _eventHubContext.All.LocationDeleted(@event.Data.Id, @event.Data.DeletedBy);
    }
}
