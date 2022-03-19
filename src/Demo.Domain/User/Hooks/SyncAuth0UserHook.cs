using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.User;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            if (context.IsPropertyDirty(x => x.Email))
            {
                context.AddMessageAsync(SyncEmailToAuth0UserMessage.Create(_correlationIdProvider.Id, context.Entity.Id), cancellationToken);
            }

            if (context.IsPropertyDirty(x => x.Fullname))
            {
                context.AddMessageAsync(SyncNameToAuth0UserMessage.Create(_correlationIdProvider.Id, context.Entity.Id), cancellationToken);
            }

            if (!Enumerable.SequenceEqual(
                context.Entity.UserRoles.Select(x => x.RoleId),
                context.Pristine.UserRoles.Select(x => x.RoleId)
            ))
            {
                context.AddMessageAsync(SyncRolesToAuth0UserMessage.Create(_correlationIdProvider.Id, context.Entity.Id), cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}