using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.Events
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
