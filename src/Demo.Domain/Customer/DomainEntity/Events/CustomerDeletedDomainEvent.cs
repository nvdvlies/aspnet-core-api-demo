using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Customer.DomainEntity.Events
{
    public class CustomerDeletedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }

        public CustomerDeletedDomainEvent(Guid id, Guid deletedBy)
        {
            Id = id;
            DeletedBy = deletedBy;
        }
    }
}
