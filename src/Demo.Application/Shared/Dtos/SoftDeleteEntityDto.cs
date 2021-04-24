using System;

namespace Demo.Application.Shared.Dtos
{
    public class SoftDeleteEntityDto : AuditableEntityDto
    {
        public bool Deleted { get; private set; }
        public Guid? DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
    }
}
