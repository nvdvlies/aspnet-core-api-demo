using Demo.Application.Shared.Interfaces;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Events.FeatureFlagSettings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.FeatureFlagSettings.Events.FeatureFlagSettingsUpdated
{
    public class FeatureFlagSettingsUpdatedEventHandler : INotificationHandler<FeatureFlagSettingsUpdatedEvent>
    {
        private readonly IFeatureFlagSettingsProvider _featureFlagSettingsProvider;
        private readonly IEventHubContext _eventHubContext;

        public FeatureFlagSettingsUpdatedEventHandler(
            IFeatureFlagSettingsProvider featureFlagSettingsProvider,
            IEventHubContext eventHubContext
        )
        {
            _featureFlagSettingsProvider = featureFlagSettingsProvider;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(FeatureFlagSettingsUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _featureFlagSettingsProvider.GetAsync(refreshCache: true, cancellationToken);
            await _eventHubContext.All.FeatureFlagSettingsUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
