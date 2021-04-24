using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.Shared.Entities
{
    public abstract class Entity : IEntity, IEquatable<IEntity>
    {
        public Guid Id { get; set; }
        public byte[] Timestamp { get; set; }

        public bool Equals(IEntity other)
        {
            if (other is null)
                return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as IEntity);
        public override int GetHashCode() => (Id).GetHashCode();
    }
}
