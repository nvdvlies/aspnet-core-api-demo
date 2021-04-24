using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<Invoice>, IBeforeUpdate<Invoice>, IBeforeDelete<Invoice>
    {
        public Task ExecuteAsync(HookType type, IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken)
        {
            context.Entity.Customer = null;
            // context.Entity.InvoiceLines?.ForEach(invoiceLine => invoiceLine.Item = null);

            return Task.CompletedTask;
        }
    }
}
