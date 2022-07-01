using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }

        // ReSharper disable once InconsistentNaming
        public uint xmin { get; set; }
    }
}