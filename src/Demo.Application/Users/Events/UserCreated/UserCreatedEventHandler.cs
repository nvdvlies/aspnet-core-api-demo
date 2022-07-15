using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Users.Events.UserCreated
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(
            ILogger<UserCreatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(UserCreatedEvent)}");
            return _eventHubContext.All.UserCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}