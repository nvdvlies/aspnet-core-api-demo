using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Customer.Events
{
    public class CustomerCreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }

        public CustomerCreatedDomainEvent(Guid id, Guid createdBy)
        {
            Id = id;
            CreatedBy = createdBy;
        }
    }
}
