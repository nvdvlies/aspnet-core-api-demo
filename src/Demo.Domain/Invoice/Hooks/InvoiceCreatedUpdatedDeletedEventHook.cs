using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Invoice;

namespace Demo.Domain.Invoice.Hooks
{
    internal class InvoiceCreatedUpdatedDeletedEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>,
        IAfterDelete<Invoice>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUser _currentUser;

        public InvoiceCreatedUpdatedDeletedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider)
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    await context.AddEventAsync(
                        InvoiceCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id),
                        cancellationToken);
                    break;
                case EditMode.Update:
                    await context.AddEventAsync(
                        InvoiceUpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id),
                        cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.AddEventAsync(
                        InvoiceDeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id),
                        cancellationToken);
                    break;
            }
        }
    }
}
