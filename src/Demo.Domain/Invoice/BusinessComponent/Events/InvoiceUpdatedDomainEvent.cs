using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.BusinessComponent.Events
{
    public class InvoiceUpdatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }

        public InvoiceUpdatedDomainEvent(Guid id, Guid updatedBy)
        {
            Id = id;
            UpdatedBy = updatedBy;
        }
    }
}
