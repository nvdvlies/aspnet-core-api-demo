using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Customer.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<Customer>, IBeforeUpdate<Customer>, IBeforeDelete<Customer>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<Customer> context, CancellationToken cancellationToken)
        {
            context.Entity.Invoices = null;

            return Task.CompletedTask;
        }
    }
}
