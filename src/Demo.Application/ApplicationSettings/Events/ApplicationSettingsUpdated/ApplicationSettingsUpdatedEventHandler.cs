using Demo.Application.Shared.Interfaces;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Events.ApplicationSettings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Events.ApplicationSettingsUpdated
{
    public class ApplicationSettingsUpdatedEventHandler : INotificationHandler<ApplicationSettingsUpdatedEvent>
    {
        private readonly IApplicationSettingsProvider _applicationSettingsProvider;
        private readonly IEventHubContext _eventHubContext;

        public ApplicationSettingsUpdatedEventHandler(
            IApplicationSettingsProvider applicationSettingsProvider,
            IEventHubContext eventHubContext
        )
        {
            _applicationSettingsProvider = applicationSettingsProvider;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(ApplicationSettingsUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _applicationSettingsProvider.GetAsync(refreshCache: true, cancellationToken);
            await _eventHubContext.All.ApplicationSettingsUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
