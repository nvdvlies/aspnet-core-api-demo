using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Role;

namespace Demo.Domain.Role.Hooks
{
    internal class RoleCreatedUpdatedDeletedEventHook : IAfterCreate<Role>, IAfterUpdate<Role>, IAfterDelete<Role>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public RoleCreatedUpdatedDeletedEventHook(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<Role> context,
            CancellationToken cancellationToken = default)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.AddEventAsync(
                        RoleCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
                case EditMode.Update:
                    context.AddEventAsync(
                        RoleUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
                case EditMode.Delete:
                    context.AddEventAsync(
                        RoleDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
