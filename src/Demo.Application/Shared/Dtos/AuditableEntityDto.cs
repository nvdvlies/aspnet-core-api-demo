using System;

namespace Demo.Application.Shared.Dtos
{
    public class AuditableEntityDto : EntityDto
    {
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Guid? LastModifiedBy { get; private set; }
        public DateTime? LastModifiedOn { get; private set; }
    }
}
