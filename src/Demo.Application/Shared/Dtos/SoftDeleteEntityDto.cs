using System;

namespace Demo.Application.Shared.Dtos
{
    public class SoftDeleteEntityDto : AuditableEntityDto
    {
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
