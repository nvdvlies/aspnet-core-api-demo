using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.User;

namespace Demo.Domain.User.Hooks
{
    internal class UserCreatedUpdatedDeletedEventHook : IAfterCreate<User>, IAfterUpdate<User>, IAfterDelete<User>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public UserCreatedUpdatedDeletedEventHook(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken = default)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.AddEventAsync(
                        UserCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
                case EditMode.Update:
                    context.AddEventAsync(
                        UserUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
                case EditMode.Delete:
                    context.AddEventAsync(
                        UserDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}