using Demo.Domain.Invoice.BusinessComponent.Events;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Hooks
{
    internal class InvoiceStatusDomainEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>
    {
        public Task ExecuteAsync(HookType type, IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken)
        {
            var isStatusPropertyDirty = context.EditMode == EditMode.Create
                || (context.EditMode == EditMode.Update && context.IsPropertyDirty(x => x.Status));

            if (isStatusPropertyDirty)
            {
                switch (context.Entity.Status)
                {
                    case InvoiceStatus.Sent:
                        context.PublishDomainEventAfterCommit(new InvoiceSentDomainEvent(context.Entity.Id));
                        break;
                    case InvoiceStatus.Paid:
                        context.PublishDomainEventAfterCommit(new InvoicePaidDomainEvent(context.Entity.Id));
                        break;
                    case InvoiceStatus.Cancelled:
                        context.PublishDomainEventAfterCommit(new InvoiceCancelledDomainEvent(context.Entity.Id));
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
