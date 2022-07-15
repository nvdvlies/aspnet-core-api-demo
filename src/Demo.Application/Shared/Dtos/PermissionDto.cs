using System;
using Demo.Application.Shared.Interfaces;

namespace Demo.Application.Shared.Dtos
{
    public class PermissionDto : IEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? PermissionGroupId { get; set; }
    }
}