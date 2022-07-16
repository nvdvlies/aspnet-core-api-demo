using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Customer.Hooks;

internal class ContextStateExampleHook : IBeforeCreate<Customer>, IBeforeUpdate<Customer>, IBeforeDelete<Customer>
{
    public Task ExecuteAsync(HookType type, IDomainEntityContext<Customer> context,
        CancellationToken cancellationToken = default)
    {
        // Example: Retrieve variable set in application layer.
        // var exampleBoolean = context.State.Get<bool>(CustomerStateKeys.ExampleBoolean);
        // or
        // var hasExampleBoolean = context.State.TryGet(CustomerStateKeys.ExampleBoolean, out bool exampleBoolean2);

        // Example: Set variable accesible in application layer after create, update or delete has been called.
        // context.State.Set(CustomerStateKeys.ExampleGuid, Guid.NewGuid());

        return Task.CompletedTask;
    }
}
