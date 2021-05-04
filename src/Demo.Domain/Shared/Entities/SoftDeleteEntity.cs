using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Shared.Entities
{
    public abstract class SoftDeleteEntity : AuditableEntity, ISoftDeleteEntity, IAuditableEntity, IEntity
    {
        public bool Deleted { get; private set; }
        public Guid? DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }

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
