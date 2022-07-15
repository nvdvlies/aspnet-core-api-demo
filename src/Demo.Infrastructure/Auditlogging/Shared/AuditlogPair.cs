using System;
using Demo.Domain.Auditlog;

namespace Demo.Infrastructure.Auditlogging.Shared
{
    internal class AuditlogPair<T>
    {
        public Guid Id { get; set; }
        public T CurrentValue { get; set; }
        public T PreviousValue { get; set; }
        public AuditlogStatus Status { get; set; }
    }
}
