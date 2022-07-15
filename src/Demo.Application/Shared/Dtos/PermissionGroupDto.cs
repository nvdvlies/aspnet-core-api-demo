using System;
using Demo.Application.Shared.Interfaces;

namespace Demo.Application.Shared.Dtos
{
    public class PermissionGroupDto : IEntityDto
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
