using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface ISoftDeleteEntity
    {
        public bool Deleted { get; }
        public Guid DeletedBy { get; }
        public DateTime? DeletedOn { get; }

        internal void MarkAsDeleted(Guid deletedBy, DateTime deletedOn);
        internal void UndoMarkAsDeleted();
    }
}