using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Invoice.Interfaces;

public interface IInvoiceDomainEntity : IDomainEntity<Invoice>
{
    void SetStatus(InvoiceStatus newStatus);
    Task GetAsNewCopyAsync(Guid id, CancellationToken cancellationToken);
    Task GetAsNewCreditAsync(Guid id, CancellationToken cancellationToken);
}
