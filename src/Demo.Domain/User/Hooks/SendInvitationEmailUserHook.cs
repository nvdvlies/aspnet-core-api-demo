using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.State;
using Demo.Messages.Email;

namespace Demo.Domain.User.Hooks
{
    internal class SendInvitationEmailUserHook : IAfterCreate<User>, IAfterUpdate<User>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public SendInvitationEmailUserHook(
            ICorrelationIdProvider correlationIdProvider,
            ICurrentUserIdProvider currentUserIdProvider
        )
        {
            _correlationIdProvider = correlationIdProvider;
            _currentUserIdProvider = currentUserIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken = default)
        {
            if (context.Entity.UserType == UserType.System)
            {
                return Task.CompletedTask;
            }

            if (!string.IsNullOrEmpty(context.Entity.ExternalId) && context.IsPropertyDirty(x => x.ExternalId))
            {
                context.State.TryGet(UserStateKeys.SkipInvitationEmailOnExternalIdChange,
                    out bool skipInvitationEmailOnExternalIdChange);

                if (!skipInvitationEmailOnExternalIdChange)
                {
                    context.AddMessageAsync(
                        SendUserInvitationEmailMessage.Create(_currentUserIdProvider.Id, _correlationIdProvider.Id,
                            context.Entity.Id), cancellationToken);
                }
            }

            return Task.CompletedTask;
        }
    }
}