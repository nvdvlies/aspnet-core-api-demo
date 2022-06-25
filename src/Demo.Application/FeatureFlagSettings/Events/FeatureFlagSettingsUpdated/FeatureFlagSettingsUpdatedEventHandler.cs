using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.FeatureFlagSettings;
using MediatR;

namespace Demo.Application.FeatureFlagSettings.Events.FeatureFlagSettingsUpdated
{
    public class FeatureFlagSettingsUpdatedEventHandler : INotificationHandler<FeatureFlagSettingsUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public FeatureFlagSettingsUpdatedEventHandler(
            IEventHubContext eventHubContext
        )
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(FeatureFlagSettingsUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.FeatureFlagSettingsUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
