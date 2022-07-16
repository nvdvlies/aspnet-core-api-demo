using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Users.Events.UserDeleted;

public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<UserDeletedEventHandler> _logger;

    public UserDeletedEventHandler(
        ILogger<UserDeletedEventHandler> logger,
        IEventHubContext eventHubContext
    )
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
    }

    public Task Handle(UserDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(UserDeletedEvent)}");
        return _eventHubContext.All.UserDeleted(@event.Data.Id, @event.Data.DeletedBy);
    }
}
