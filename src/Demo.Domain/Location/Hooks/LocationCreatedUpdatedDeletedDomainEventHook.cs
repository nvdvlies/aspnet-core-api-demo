using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Location;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Location.Hooks
{
    internal class LocationCreatedUpdatedDeletedEventHook : IAfterCreate<Location>, IAfterUpdate<Location>, IAfterDelete<Location>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public LocationCreatedUpdatedDeletedEventHook(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<Location> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    await context.AddEventAsync(LocationCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUserIdProvider.Id), cancellationToken);
                    break;
                case EditMode.Update:
                    await context.AddEventAsync(LocationUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUserIdProvider.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.AddEventAsync(LocationDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUserIdProvider.Id), cancellationToken);
                    break;
            }
        }
    }
}