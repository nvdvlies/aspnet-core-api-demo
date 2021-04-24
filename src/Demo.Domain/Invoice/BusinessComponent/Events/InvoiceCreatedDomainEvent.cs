using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.BusinessComponent.Events
{
    public class InvoiceCreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }

        public InvoiceCreatedDomainEvent(Guid id, Guid createdBy)
        {
            Id = id;
            CreatedBy = createdBy;
        }
    }
}
