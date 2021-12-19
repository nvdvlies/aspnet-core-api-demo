using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Invoice.DomainEntity.Events
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
