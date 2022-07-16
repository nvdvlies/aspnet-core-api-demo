using System.Collections.Generic;

namespace Demo.Domain.Auditlog;

public class AuditlogItem
{
    public string PropertyName { get; set; }
    public AuditlogStatus Status { get; set; }
    public AuditlogType Type { get; set; }
    public string CurrentValueAsString { get; set; }
    public string PreviousValueAsString { get; set; }
    public List<AuditlogItem> AuditlogItems { get; set; }
}
