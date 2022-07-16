using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.UserPreferences;

namespace Demo.Domain.UserPreferences.Hooks;

internal class UserPreferencesCreatedUpdatedDeletedEventHook : IAfterCreate<UserPreferences>,
    IAfterUpdate<UserPreferences>, IAfterDelete<UserPreferences>
{
    private readonly ICorrelationIdProvider _correlationIdProvider;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public UserPreferencesCreatedUpdatedDeletedEventHook(
        ICurrentUserIdProvider currentUserIdProvider,
        ICorrelationIdProvider correlationIdProvider
    )
    {
        _currentUserIdProvider = currentUserIdProvider;
        _correlationIdProvider = correlationIdProvider;
    }

    public async Task ExecuteAsync(HookType type, IDomainEntityContext<UserPreferences> context,
        CancellationToken cancellationToken = default)
    {
        switch (context.EditMode)
        {
            case EditMode.Create:
            case EditMode.Update:
                await context.AddEventAsync(
                    UserPreferencesUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                        _currentUserIdProvider.Id), cancellationToken);
                break;
            case EditMode.Delete:
                await context.AddEventAsync(
                    UserPreferencesDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                        _currentUserIdProvider.Id), cancellationToken);
                break;
        }
    }
}
