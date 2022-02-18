using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Demo.Domain.Auditlog
{
    public partial class Auditlog : Entity, IQueryableEntity
    {
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<AuditlogItem> AuditlogItems { get; set; }
    }
}
