using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.BusinessComponent.Interfaces
{
    public interface IInvoiceBusinessComponent : IBusinessComponent<Invoice>
    {
        void SetStatus(InvoiceStatus newStatus);
    }
}
