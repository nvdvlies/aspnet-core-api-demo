using System;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Shared.Entities
{
    public abstract class Entity : IEntity, IEquatable<IEntity>
    {
        public Guid Id { get; set; }

        // ReSharper disable once InconsistentNaming
        public uint xmin { get; set; }

        public bool Equals(IEntity other)
        {
            if (other is null)
            {
                return false;
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IEntity);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }
    }
}