using Demo.Domain.Shared.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.Shared.Entities
{
    public abstract class AuditableEntity : Entity, IAuditableEntity, IEntity
    {
        [JsonInclude]
        public Guid CreatedBy { get; private set; }
        [JsonInclude]
        public DateTime CreatedOn { get; private set; }
        [JsonInclude]
        public Guid? LastModifiedBy { get; private set; }
        [JsonInclude]
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

        void IAuditableEntity.ClearCreatedAndLastModified()
        {
            CreatedBy = default;
            CreatedOn = default;
            LastModifiedBy = default;
            LastModifiedOn = default;
        }
    }
}
