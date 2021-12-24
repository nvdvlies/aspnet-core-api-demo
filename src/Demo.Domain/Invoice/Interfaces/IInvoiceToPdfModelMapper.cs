using Demo.Domain.Invoice.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.Interfaces
{
    public interface IInvoiceToPdfModelMapper
    {
        Task<InvoiceToPdfModel> MapAsync(Invoice invoice, CancellationToken cancellationToken);
    }
}
