using System;
using Demo.Application.Shared.Interfaces;

namespace Demo.Application.Shared.Dtos
{
    public class PermissionGroupDto : IEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}