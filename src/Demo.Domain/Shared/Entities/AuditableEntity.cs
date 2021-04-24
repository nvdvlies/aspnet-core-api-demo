using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Shared.Entities
{
    public abstract class AuditableEntity : Entity, IAuditableEntity, IEntity
    {
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Guid? LastModifiedBy { get; private set; }
        public DateTime? LastModifiedOn { get; private set; }

        void IAuditableEntity.SetCreatedByAndCreatedOn(Guid createdBy, DateTime createdOn)
        {
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            LastModifiedBy = createdBy;
            LastModifiedOn = createdOn;
        }

        void IAuditableEntity.SetLastModifiedByAndLastModifiedOn(Guid lastModifiedBy, DateTime lastModifiedOn)
        {
            LastModifiedBy = lastModifiedBy;
            LastModifiedOn = lastModifiedOn;
        }
    }
}
