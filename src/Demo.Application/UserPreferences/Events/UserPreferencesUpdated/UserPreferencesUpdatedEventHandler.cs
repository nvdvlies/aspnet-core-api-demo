using Demo.Application.Shared.Interfaces;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Events.ApplicationSettings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.UserPreferences.Interfaces;
using Demo.Events.UserPreferences;

namespace Demo.Application.UserPreferences.Events.UserPreferencesUpdated
{
    public class UserPreferencesUpdatedEventHandler : INotificationHandler<UserPreferencesUpdatedEvent>
    {
        private readonly IUserPreferencesProvider _userPreferencesProvider;
        private readonly IEventHubContext _eventHubContext;

        public UserPreferencesUpdatedEventHandler(
            IUserPreferencesProvider userPreferencesProvider,
            IEventHubContext eventHubContext
        )
        {
            _userPreferencesProvider = userPreferencesProvider;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(UserPreferencesUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _userPreferencesProvider.GetAsync(refreshCache: true, cancellationToken);
            await _eventHubContext.All.UserPreferencesUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
