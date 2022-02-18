using Demo.Domain.Shared.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.Shared.Entities
{
    public abstract class SoftDeleteEntity : AuditableEntity, ISoftDeleteEntity, IAuditableEntity, IEntity
    {
        [JsonInclude]
        public bool Deleted { get; private set; }
        [JsonInclude]
        public string DeletedBy { get; private set; }
        [JsonInclude]
        public DateTime? DeletedOn { get; private set; }

        void ISoftDeleteEntity.MarkAsDeleted(string deletedBy, DateTime deletedOn)
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
