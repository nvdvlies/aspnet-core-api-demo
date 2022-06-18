using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.UserPreferences.Interfaces;
using Demo.Events.UserPreferences;
using MediatR;

namespace Demo.Application.UserPreferences.Events.UserPreferencesUpdated
{
    public class UserPreferencesUpdatedEventHandler : INotificationHandler<UserPreferencesUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly IUserPreferencesProvider _userPreferencesProvider;

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
            await _userPreferencesProvider.GetAsync(true, cancellationToken);
            await _eventHubContext.All.UserPreferencesUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
