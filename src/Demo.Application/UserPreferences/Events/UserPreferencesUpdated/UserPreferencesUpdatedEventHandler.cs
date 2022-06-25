using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.UserPreferences;
using MediatR;

namespace Demo.Application.UserPreferences.Events.UserPreferencesUpdated
{
    public class UserPreferencesUpdatedEventHandler : INotificationHandler<UserPreferencesUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public UserPreferencesUpdatedEventHandler(
            IEventHubContext eventHubContext
        )
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(UserPreferencesUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.UserPreferencesUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
