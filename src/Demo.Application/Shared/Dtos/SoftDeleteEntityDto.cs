using System;

namespace Demo.Application.Shared.Dtos
{
    public class SoftDeleteEntityDto : AuditableEntityDto
    {
        public bool Deleted { get; private set; }
        public string DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
    }
}
