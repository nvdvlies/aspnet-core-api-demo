using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.Events
{
    public class InvoicePdfSynchronizedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public InvoicePdfSynchronizedDomainEvent(Guid id)
        {
            Id = id;
        }
    }
}
