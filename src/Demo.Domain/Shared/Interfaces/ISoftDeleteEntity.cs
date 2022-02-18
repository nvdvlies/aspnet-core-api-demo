using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface ISoftDeleteEntity
    {
        public bool Deleted { get; }
        public string DeletedBy { get; }
        public DateTime? DeletedOn { get; }

        internal void MarkAsDeleted(string deletedBy, DateTime deletedOn);
        internal void UndoMarkAsDeleted();
    }
}
