using System;
using Demo.Application.Shared.Interfaces;

namespace Demo.Application.Shared.Dtos
{
    public class EntityDto : IEntityDto
    {
        // ReSharper disable once InconsistentNaming
        public uint xmin { get; set; }
        public Guid Id { get; set; }
    }
}
