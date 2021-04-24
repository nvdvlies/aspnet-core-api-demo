using Demo.Application.Shared.Interfaces;
using System;

namespace Demo.Application.Shared.Dtos
{
    public class EntityDto : IEntityDto
    {
        public Guid Id { get; set; }

        public byte[] Timestamp { get; set; }
    }
}
