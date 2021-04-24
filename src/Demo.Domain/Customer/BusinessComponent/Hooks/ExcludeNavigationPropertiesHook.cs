using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Customer.BusinessComponent.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<Customer>, IBeforeUpdate<Customer>, IBeforeDelete<Customer>
    {
        public Task ExecuteAsync(HookType type, IBusinessComponentContext<Customer> context, CancellationToken cancellationToken)
        {
            context.Entity.Invoices = null;

            return Task.CompletedTask;
        }
    }
}
