using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.UserPreferences;

namespace Demo.Domain.UserPreferences.Hooks
{
    internal class UserPreferencesCreatedUpdatedDeletedEventHook : IAfterCreate<UserPreferences>,
        IAfterUpdate<UserPreferences>, IAfterDelete<UserPreferences>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUser _currentUser;

        public UserPreferencesCreatedUpdatedDeletedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<UserPreferences> context,
            CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                case EditMode.Update:
                    await context.AddEventAsync(
                        UserPreferencesUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.AddEventAsync(
                        UserPreferencesDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUser.Id), cancellationToken);
                    break;
            }
        }
    }
}