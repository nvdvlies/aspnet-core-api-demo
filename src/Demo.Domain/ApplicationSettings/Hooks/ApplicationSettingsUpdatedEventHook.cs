using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.ApplicationSettings;

namespace Demo.Domain.ApplicationSettings.Hooks
{
    internal class ApplicationSettingsUpdatedEventHook : IAfterCreate<ApplicationSettings>,
        IAfterUpdate<ApplicationSettings>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public ApplicationSettingsUpdatedEventHook(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<ApplicationSettings> context,
            CancellationToken cancellationToken = default)
        {
            return context.AddEventAsync(
                ApplicationSettingsUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                    _currentUserIdProvider.Id),
                cancellationToken);
        }
    }
}