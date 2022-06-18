using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Invoice;

namespace Demo.Domain.Invoice.Hooks
{
    internal class InvoiceStatusEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public InvoiceStatusEventHook(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken)
        {
            var isStatusPropertyDirty = context.EditMode == EditMode.Create
                                        || (context.EditMode == EditMode.Update &&
                                            context.IsPropertyDirty(x => x.Status));

            if (isStatusPropertyDirty)
            {
                switch (context.Entity.Status)
                {
                    case InvoiceStatus.Sent:
                        await context.AddEventAsync(
                            InvoiceSentEvent.Create(_correlationIdProvider.Id, context.Entity.Id), cancellationToken);
                        break;
                    case InvoiceStatus.Paid:
                        await context.AddEventAsync(
                            InvoicePaidEvent.Create(_correlationIdProvider.Id, context.Entity.Id), cancellationToken);
                        break;
                    case InvoiceStatus.Cancelled:
                        await context.AddEventAsync(
                            InvoiceCancelledEvent.Create(_correlationIdProvider.Id, context.Entity.Id),
                            cancellationToken);
                        break;
                }
            }
        }
    }
}
