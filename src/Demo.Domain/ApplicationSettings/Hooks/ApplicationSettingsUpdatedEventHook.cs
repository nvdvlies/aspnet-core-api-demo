using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.ApplicationSettings;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.Hooks
{
    internal class ApplicationSettingsUpdatedEventHook : IAfterCreate<ApplicationSettings>, IAfterUpdate<ApplicationSettings>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public ApplicationSettingsUpdatedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<ApplicationSettings> context, CancellationToken cancellationToken)
        {
            context.PublishIntegrationEvent(ApplicationSettingsUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id));
            return Task.CompletedTask;
        }
    }
}
