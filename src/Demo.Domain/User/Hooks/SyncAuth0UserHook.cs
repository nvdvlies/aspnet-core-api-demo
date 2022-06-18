using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.User;

namespace Demo.Domain.User.Hooks
{
    internal class SyncAuth0UserHook : IAfterUpdate<User>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public SyncAuth0UserHook(
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            if (context.IsPropertyDirty(x => x.Email)
                || context.IsPropertyDirty(x => x.Fullname)
                || !context.Entity.UserRoles.OrderBy(x => x.RoleId).Select(x => x.RoleId).SequenceEqual(
                    context.Pristine.UserRoles.OrderBy(x => x.RoleId).Select(x => x.RoleId)
                ))
            {
                context.AddMessageAsync(
                    SyncAuth0UserMessage.Create(_correlationIdProvider.Id, context.Entity.Id,
                        context.IsPropertyDirty(x => x.Email)), cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
