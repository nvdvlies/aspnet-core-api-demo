using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Invoice;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.Hooks
{
    internal class InvoiceCreatedUpdatedDeletedEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>, IAfterDelete<Invoice>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public InvoiceCreatedUpdatedDeletedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider)
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    await context.PublishIntegrationEventAsync(InvoiceCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Update:
                    await context.PublishIntegrationEventAsync(InvoiceUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.PublishIntegrationEventAsync(InvoiceDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
            }
        }
    }
}
