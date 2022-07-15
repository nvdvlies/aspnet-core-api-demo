using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.UserPreferences;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.UserPreferences.Events.UserPreferencesUpdated
{
    public class UserPreferencesUpdatedEventHandler : INotificationHandler<UserPreferencesUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<UserPreferencesUpdatedEventHandler> _logger;

        public UserPreferencesUpdatedEventHandler(
            ILogger<UserPreferencesUpdatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(UserPreferencesUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(UserPreferencesUpdatedEvent)}");
            return _eventHubContext.All.UserPreferencesUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
