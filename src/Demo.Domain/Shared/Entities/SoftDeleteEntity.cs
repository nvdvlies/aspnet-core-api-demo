using System;
using System.Text.Json.Serialization;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Shared.Entities
{
    public abstract class SoftDeleteEntity : AuditableEntity, ISoftDeleteEntity, IAuditableEntity, IEntity
    {
        [JsonInclude] public bool Deleted { get; private set; }

        [JsonInclude] public Guid DeletedBy { get; private set; }

        [JsonInclude] public DateTime? DeletedOn { get; private set; }

        void ISoftDeleteEntity.MarkAsDeleted(Guid deletedBy, DateTime deletedOn)
        {
            Deleted = true;
            DeletedBy = deletedBy;
            DeletedOn = deletedOn;
        }

        void ISoftDeleteEntity.UndoMarkAsDeleted()
        {
            Deleted = false;
            DeletedBy = default;
            DeletedOn = default;
        }
    }
}
