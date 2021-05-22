using Demo.Application.Shared.Interfaces;
using Demo.Domain.ApplicationSettings.BusinessComponent.Events;
using Demo.Domain.ApplicationSettings.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Events.ApplicationSettingsUpdated
{
    public class ApplicationSettingsUpdatedDomainEventHandler : INotificationHandler<ApplicationSettingsUpdatedDomainEvent>
    {
        private readonly IApplicationSettingsProvider _applicationSettingsProvider;
        private readonly IEventHubContext _eventHubContext;

        public ApplicationSettingsUpdatedDomainEventHandler(
            IApplicationSettingsProvider applicationSettingsProvider,
            IEventHubContext eventHubContext
        )
        {
            _applicationSettingsProvider = applicationSettingsProvider;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(ApplicationSettingsUpdatedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _applicationSettingsProvider.GetAsync(refreshCache: true, cancellationToken);
            await _eventHubContext.All.ApplicationSettingsUpdated(@event.Id, @event.UpdatedBy);
        }
    }
}
