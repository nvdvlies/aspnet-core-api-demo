using System;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events
{
    public interface IInvoiceEventHub
    {
        Task InvoiceCreated(Guid id, Guid createdBy);
        Task InvoiceUpdated(Guid id, Guid updatedBy);
        Task InvoiceDeleted(Guid id, Guid deletedBy);
        Task InvoiceCancelled(Guid id);
        Task InvoicePaid(Guid id);
        Task InvoiceSent(Guid id);
        Task InvoicePdfSynchronized(Guid id);
    }
}