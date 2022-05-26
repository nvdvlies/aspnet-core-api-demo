using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.User.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<User>, IBeforeUpdate<User>, IBeforeDelete<User>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            context.Entity.UserRoles?.ForEach(x =>
            {
                x.Role = null;
            });

            return Task.CompletedTask;
        }
    }
}
