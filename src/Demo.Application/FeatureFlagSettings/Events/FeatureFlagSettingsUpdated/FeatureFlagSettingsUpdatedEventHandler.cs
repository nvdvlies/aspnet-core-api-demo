using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.FeatureFlagSettings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.FeatureFlagSettings.Events.FeatureFlagSettingsUpdated
{
    public class FeatureFlagSettingsUpdatedEventHandler : INotificationHandler<FeatureFlagSettingsUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<FeatureFlagSettingsUpdatedEventHandler> _logger;

        public FeatureFlagSettingsUpdatedEventHandler(
            ILogger<FeatureFlagSettingsUpdatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(FeatureFlagSettingsUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(FeatureFlagSettingsUpdatedEvent)}");
            return _eventHubContext.All.FeatureFlagSettingsUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
