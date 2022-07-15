using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<UserPreferences>, IBeforeUpdate<UserPreferences>,
        IBeforeDelete<UserPreferences>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<UserPreferences> context,
            CancellationToken cancellationToken = default)
        {
            context.Entity.User = null;

            return Task.CompletedTask;
        }
    }
}
