using System;

namespace Demo.Application.Shared.Dtos
{
    public class AuditableEntityDto : EntityDto
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
