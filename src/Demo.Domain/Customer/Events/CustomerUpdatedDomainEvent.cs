using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Customer.Events
{
    public class CustomerUpdatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }

        public CustomerUpdatedDomainEvent(Guid id, Guid updatedBy)
        {
            Id = id;
            UpdatedBy = updatedBy;
        }
    }
}
