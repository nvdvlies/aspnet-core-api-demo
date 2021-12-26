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

        public Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.PublishIntegrationEvent(InvoiceCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Update:
                    context.PublishIntegrationEvent(InvoiceUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Delete:
                    context.PublishIntegrationEvent(InvoiceDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id));
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
