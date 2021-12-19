using Demo.Domain.Invoice.DomainEntity.Events;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.DomainEntity.Hooks
{
    internal class InvoiceStatusDomainEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context, CancellationToken cancellationToken)
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
