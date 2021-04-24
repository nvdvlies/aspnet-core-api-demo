using Demo.Domain.Invoice.BusinessComponent.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Interfaces
{
    public interface IInvoiceToPdfModelMapper
    {
        Task<InvoiceToPdfModel> MapAsync(Invoice invoice, CancellationToken cancellationToken);
    }
}
