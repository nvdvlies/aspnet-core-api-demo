using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.Events
{
    public class InvoiceSentDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public InvoiceSentDomainEvent(Guid id)
        {
            Id = id;
        }
    }
}
