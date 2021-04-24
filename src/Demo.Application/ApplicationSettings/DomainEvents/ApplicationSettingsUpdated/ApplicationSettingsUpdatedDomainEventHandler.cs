using Demo.Domain.ApplicationSettings.BusinessComponent.Events;
using Demo.Domain.ApplicationSettings.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.DomainEvents.ApplicationSettingsUpdated
{
    public class ApplicationSettingsUpdatedDomainEventHandler : INotificationHandler<ApplicationSettingsUpdatedDomainEvent>
    {
        private readonly IApplicationSettingsProvider _applicationSettingsProvider;

        public ApplicationSettingsUpdatedDomainEventHandler(
            IApplicationSettingsProvider applicationSettingsProvider
        )
        {
            _applicationSettingsProvider = applicationSettingsProvider;
        }

        public async Task Handle(ApplicationSettingsUpdatedDomainEvent @event, CancellationToken cancellationToken)
        {
            await _applicationSettingsProvider.GetAsync(refreshCache: true, cancellationToken);
        }
    }
}
