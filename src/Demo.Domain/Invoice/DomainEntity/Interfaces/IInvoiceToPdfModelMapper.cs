using Demo.Domain.Invoice.DomainEntity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.DomainEntity.Interfaces
{
    public interface IInvoiceToPdfModelMapper
    {
        Task<InvoiceToPdfModel> MapAsync(Invoice invoice, CancellationToken cancellationToken);
    }
}
