using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Invoice.Hooks
{
    internal class SetInvoiceLineNumbersHook : IBeforeCreate<Invoice>, IBeforeUpdate<Invoice>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken)
        {
            var i = 0;
            foreach (var invoiceLine in context.Entity.InvoiceLines)
            {
                invoiceLine.LineNumber = ++i;
            }

            return Task.CompletedTask;
        }
    }
}
