using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.BusinessComponent.Events
{
    public class InvoicePaidDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public InvoicePaidDomainEvent(Guid id)
        {
            Id = id;
        }
    }
}
