using Demo.Common.Interfaces;
using Demo.Domain.Invoice.DomainEntity.Events;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.DomainEntity.Hooks
{
    internal class InvoiceCreatedUpdatedDeletedDomainEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>, IAfterDelete<Invoice>
    {
        private readonly ICurrentUser _currentUser;

        public InvoiceCreatedUpdatedDeletedDomainEventHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<Invoice> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.PublishDomainEventAfterCommit(new InvoiceCreatedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Update:
                    context.PublishDomainEventAfterCommit(new InvoiceUpdatedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Delete:
                    context.PublishDomainEventAfterCommit(new InvoiceDeletedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
