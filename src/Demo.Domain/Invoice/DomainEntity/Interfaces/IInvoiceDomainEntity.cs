using Demo.Domain.Shared.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.DomainEntity.Interfaces
{
    public interface IInvoiceDomainEntity : IDomainEntity<Invoice>
    {
        void SetStatus(InvoiceStatus newStatus);
        Task GetAsNewCopyAsync(Guid id, CancellationToken cancellationToken);
        Task GetAsNewCreditAsync(Guid id, CancellationToken cancellationToken);
    }
}
