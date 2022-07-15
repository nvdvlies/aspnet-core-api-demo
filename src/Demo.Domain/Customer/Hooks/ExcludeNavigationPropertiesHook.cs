using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Customer.Hooks
{
    internal class ExcludeNavigationPropertiesHook : IBeforeCreate<Customer>, IBeforeUpdate<Customer>,
        IBeforeDelete<Customer>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<Customer> context,
            CancellationToken cancellationToken = default)
        {
            context.Entity.Invoices = null;

            return Task.CompletedTask;
        }
    }
}