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
        private readonly ICurrentUser _currentUser;

        public ApplicationSettingsUpdatedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<ApplicationSettings> context,
            CancellationToken cancellationToken)
        {
            await context.AddEventAsync(
                ApplicationSettingsUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id),
                cancellationToken);
        }
    }
}
