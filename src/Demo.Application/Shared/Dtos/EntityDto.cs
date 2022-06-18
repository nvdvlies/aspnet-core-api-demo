using System;
using Demo.Application.Shared.Interfaces;

namespace Demo.Application.Shared.Dtos
{
    public class EntityDto : IEntityDto
    {
        public byte[] Timestamp { get; set; }
        public Guid Id { get; set; }
    }
}
