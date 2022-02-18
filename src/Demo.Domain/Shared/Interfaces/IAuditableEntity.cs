using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        public string CreatedBy { get; }

        public DateTime CreatedOn { get; }

        public string LastModifiedBy { get; }

        public DateTime? LastModifiedOn { get; }

        internal void SetCreatedByAndCreatedOn(string createdBy, DateTime createdOn);
        internal void SetLastModifiedByAndLastModifiedOn(string lastModifiedBy, DateTime lastModifiedOn);
        internal void ClearCreatedAndLastModified();
    }
}
