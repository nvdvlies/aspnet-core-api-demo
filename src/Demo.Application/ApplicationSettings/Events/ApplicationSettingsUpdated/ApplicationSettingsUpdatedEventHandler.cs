using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.ApplicationSettings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.ApplicationSettings.Events.ApplicationSettingsUpdated
{
    public class ApplicationSettingsUpdatedEventHandler : INotificationHandler<ApplicationSettingsUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<ApplicationSettingsUpdatedEventHandler> _logger;

        public ApplicationSettingsUpdatedEventHandler(
            ILogger<ApplicationSettingsUpdatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(ApplicationSettingsUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(ApplicationSettingsUpdatedEvent)}");
            return _eventHubContext.All.ApplicationSettingsUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
