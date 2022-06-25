using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Customer;

namespace Demo.Domain.Customer.Hooks
{
    internal class CustomerCreatedUpdatedDeletedEventHook : IAfterCreate<Customer>, IAfterUpdate<Customer>,
        IAfterDelete<Customer>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public CustomerCreatedUpdatedDeletedEventHook(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<Customer> context,
            CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    await context.AddEventAsync(
                        CustomerCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
                case EditMode.Update:
                    await context.AddEventAsync(
                        CustomerUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.AddEventAsync(
                        CustomerDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                            _currentUserIdProvider.Id),
                        cancellationToken);
                    break;
            }
        }
    }
}
