using Demo.Domain.Shared.Entities;
using System;
using System.Collections.Generic;

namespace Demo.Domain.Auditlog
{
    public partial class AuditlogItem : Entity
    {
        public string PropertyName { get; set; }
        public AuditlogStatus Status { get; set; }
        public AuditlogType Type { get; set; }
        public string CurrentValueAsString { get; set; }
        public string PreviousValueAsString { get; set; }

        public Guid? ParentAuditlogItemId { get; set; }
        public AuditlogItem ParentAuditlogItem { get; set; }

        public Guid? AuditlogId { get; set; }
        public Auditlog Auditlog { get; set; }

        public List<AuditlogItem> AuditlogItems { get; set; }
    }
}
