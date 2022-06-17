using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public byte[] Timestamp { get; set; }
    }
}