using System;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events
{
    public interface IInvoiceEventHub
    {
        Task InvoiceCreated(Guid id, string createdBy);
        Task InvoiceUpdated(Guid id, string updatedBy);
        Task InvoiceDeleted(Guid id, string deletedBy);
        Task InvoiceCancelled(Guid id);
        Task InvoicePaid(Guid id);
        Task InvoiceSent(Guid id);
        Task InvoicePdfSynchronized(Guid id);
    }
}
