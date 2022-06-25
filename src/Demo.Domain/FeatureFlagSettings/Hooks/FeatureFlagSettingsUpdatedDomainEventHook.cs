using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.FeatureFlagSettings;

namespace Demo.Domain.FeatureFlagSettings.Hooks
{
    internal class FeatureFlagSettingsUpdatedDomainEventHook : IAfterCreate<FeatureFlagSettings>,
        IAfterUpdate<FeatureFlagSettings>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public FeatureFlagSettingsUpdatedDomainEventHook(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<FeatureFlagSettings> context,
            CancellationToken cancellationToken)
        {
            await context.AddEventAsync(
                FeatureFlagSettingsUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                    _currentUserIdProvider.Id),
                cancellationToken);
        }
    }
}
