using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Invoice.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<Invoice>, IBeforeUpdate<Invoice>,
        IBeforeDelete<Invoice>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken)
        {
            context.Entity.Customer = null;
            // context.Entity.InvoiceLines?.ForEach(invoiceLine => invoiceLine.Item = null);

            return Task.CompletedTask;
        }
    }
}