using Demo.Common.Interfaces;
using Demo.Domain.Invoice.BusinessComponent.Events;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Hooks
{
    internal class CreateUpdateDeleteInvoiceDomainEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>, IAfterDelete<Invoice>
    {
        private readonly ICurrentUser _currentUser;

        public CreateUpdateDeleteInvoiceDomainEventHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken)
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
