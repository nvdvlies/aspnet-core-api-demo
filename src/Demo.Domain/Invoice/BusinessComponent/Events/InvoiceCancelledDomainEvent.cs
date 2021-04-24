using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.BusinessComponent.Events
{
    public class InvoiceCancelledDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public InvoiceCancelledDomainEvent(Guid id)
        {
            Id = id;
        }
    }
}
