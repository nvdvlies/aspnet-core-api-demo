using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Role.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<Role>, IBeforeUpdate<Role>, IBeforeDelete<Role>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<Role> context, CancellationToken cancellationToken)
        {
            context.Entity.UserRoles = null;
            context.Entity.RolePermissions?.ForEach(x => { x.Permission = null; });

            return Task.CompletedTask;
        }
    }
}