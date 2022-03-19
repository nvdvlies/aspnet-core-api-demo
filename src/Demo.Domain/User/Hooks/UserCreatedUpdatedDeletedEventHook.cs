using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.User;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.User.Hooks
{
    internal class UserCreatedUpdatedDeletedEventHook : IAfterCreate<User>, IAfterUpdate<User>, IAfterDelete<User>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public UserCreatedUpdatedDeletedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.AddEventAsync(UserCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Update:
                    context.AddEventAsync(UserUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    context.AddEventAsync(UserDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}