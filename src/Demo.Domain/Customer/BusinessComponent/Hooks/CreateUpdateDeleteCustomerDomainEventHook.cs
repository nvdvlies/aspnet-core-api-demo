using Demo.Common.Interfaces;
using Demo.Domain.Customer.BusinessComponent.Events;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Customer.BusinessComponent.Hooks
{
    internal class CreateUpdateDeleteCustomerDomainEventHook : IAfterCreate<Customer>, IAfterUpdate<Customer>, IAfterDelete<Customer>
    {
        private readonly ICurrentUser _currentUser;

        public CreateUpdateDeleteCustomerDomainEventHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IBusinessComponentContext<Customer> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.PublishDomainEventAfterCommit(new CustomerCreatedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Update:
                    context.PublishDomainEventAfterCommit(new CustomerUpdatedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Delete:
                    context.PublishDomainEventAfterCommit(new CustomerDeletedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
