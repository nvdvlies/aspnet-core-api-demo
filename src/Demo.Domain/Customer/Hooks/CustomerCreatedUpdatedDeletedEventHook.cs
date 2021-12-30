using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Customer;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Customer.Hooks
{
    internal class CustomerCreatedUpdatedDeletedEventHook : IAfterCreate<Customer>, IAfterUpdate<Customer>, IAfterDelete<Customer>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CustomerCreatedUpdatedDeletedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<Customer> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    await context.PublishIntegrationEventAsync(CustomerCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Update:
                    await context.PublishIntegrationEventAsync(CustomerUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.PublishIntegrationEventAsync(CustomerDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
            }
        }
    }
}
