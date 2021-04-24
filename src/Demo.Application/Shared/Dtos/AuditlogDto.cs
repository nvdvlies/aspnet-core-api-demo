using System;
using System.Collections.Generic;

namespace Demo.Application.Shared.Dtos
{
    public class AuditlogDto : EntityDto
    {
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public List<AuditlogItemDto> AuditlogItems { get; set; }
    }
}
