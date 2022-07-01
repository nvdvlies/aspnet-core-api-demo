using System;
using System.Collections.Generic;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Auditlog
{
    public class Auditlog : Entity, IQueryableEntity
    {
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<AuditlogItem> AuditlogItems { get; set; }
    }
}