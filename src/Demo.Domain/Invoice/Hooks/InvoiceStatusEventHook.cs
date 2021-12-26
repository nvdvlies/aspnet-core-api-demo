using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Invoice;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.Hooks
{
    internal class InvoiceStatusEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public InvoiceStatusEventHook(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context, CancellationToken cancellationToken)
        {
            var isStatusPropertyDirty = context.EditMode == EditMode.Create
                || context.EditMode == EditMode.Update && context.IsPropertyDirty(x => x.Status);

            if (isStatusPropertyDirty)
            {
                switch (context.Entity.Status)
                {
                    case InvoiceStatus.Sent:
                        context.PublishIntegrationEvent(InvoiceSentEvent.Create(_correlationIdProvider.Id, context.Entity.Id));
                        break;
                    case InvoiceStatus.Paid:
                        context.PublishIntegrationEvent(InvoicePaidEvent.Create(_correlationIdProvider.Id, context.Entity.Id));
                        break;
                    case InvoiceStatus.Cancelled:
                        context.PublishIntegrationEvent(InvoiceCancelledEvent.Create(_correlationIdProvider.Id, context.Entity.Id));
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
