using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.FeatureFlagSettings;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.FeatureFlagSettings.Hooks
{
    internal class FeatureFlagSettingsUpdatedDomainEventHook : IAfterCreate<FeatureFlagSettings>, IAfterUpdate<FeatureFlagSettings>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public FeatureFlagSettingsUpdatedDomainEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<FeatureFlagSettings> context, CancellationToken cancellationToken)
        {
            await context.AddEventAsync(FeatureFlagSettingsUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
        }
    }
}