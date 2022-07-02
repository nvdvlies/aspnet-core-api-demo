using System.Collections.Generic;

namespace Demo.Application.Shared.Dtos
{
    public class AuditlogItemDto
    {
        public string PropertyName { get; set; }
        public AuditlogStatusEnum Status { get; set; }
        public AuditlogTypeEnum Type { get; set; }
        public string CurrentValueAsString { get; set; }
        public string PreviousValueAsString { get; set; }
        public List<AuditlogItemDto> AuditlogItems { get; set; }
    }
}