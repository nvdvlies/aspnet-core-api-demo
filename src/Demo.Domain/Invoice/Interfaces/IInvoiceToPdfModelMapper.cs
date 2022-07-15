using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice.Models;

namespace Demo.Domain.Invoice.Interfaces
{
    public interface IInvoiceToPdfModelMapper
    {
        Task<InvoiceToPdfModel> MapAsync(Invoice invoice, CancellationToken cancellationToken);
    }
}
