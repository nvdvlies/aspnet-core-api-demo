using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.ApplicationSettings;
using MediatR;

namespace Demo.Application.ApplicationSettings.Events.ApplicationSettingsUpdated
{
    public class ApplicationSettingsUpdatedEventHandler : INotificationHandler<ApplicationSettingsUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public ApplicationSettingsUpdatedEventHandler(
            IEventHubContext eventHubContext
        )
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(ApplicationSettingsUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.ApplicationSettingsUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
