using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.ApplicationSettings.BusinessComponent.Events
{
    public class ApplicationSettingsUpdatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }

        public ApplicationSettingsUpdatedDomainEvent(Guid id, Guid updatedBy)
        {
            Id = id;
            UpdatedBy = updatedBy;
        }
    }
}
