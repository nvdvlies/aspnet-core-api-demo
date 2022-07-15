using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.User;

namespace Demo.Domain.User.Hooks
{
    internal class DeleteAuth0UserHook : IAfterDelete<User>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public DeleteAuth0UserHook(
            ICorrelationIdProvider correlationIdProvider,
            ICurrentUserIdProvider currentUserIdProvider
        )
        {
            _correlationIdProvider = correlationIdProvider;
            _currentUserIdProvider = currentUserIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context,
            CancellationToken cancellationToken = default)
        {
            if (context.Entity.UserType == UserType.System)
            {
                return Task.CompletedTask;
            }

            context.AddMessageAsync(
                DeleteAuth0UserMessage.Create(_currentUserIdProvider.Id, _correlationIdProvider.Id, context.Entity.Id),
                cancellationToken);
            return Task.CompletedTask;
        }
    }
}
