using System;

namespace Demo.Application.Shared.Dtos
{
    public class AuditableEntityDto : EntityDto
    {
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string LastModifiedBy { get; private set; }
        public DateTime? LastModifiedOn { get; private set; }
    }
}
