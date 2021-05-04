using Demo.Domain.Shared.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Interfaces
{
    public interface IInvoiceBusinessComponent : IBusinessComponent<Invoice>
    {
        void SetStatus(InvoiceStatus newStatus);
        Task GetCopyAsync(Guid id, CancellationToken cancellationToken);
    }
}
