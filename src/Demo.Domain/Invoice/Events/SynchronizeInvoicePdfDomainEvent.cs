using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.Events
{
    public class SynchronizeInvoicePdfDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public SynchronizeInvoicePdfDomainEvent(Guid id)
        {
            Id = id;
        }
    }
}
