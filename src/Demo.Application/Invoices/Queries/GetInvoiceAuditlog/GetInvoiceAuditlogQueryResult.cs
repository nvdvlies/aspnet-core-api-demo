using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;
using System;
using System.Collections.Generic;

namespace Demo.Application.Invoices.Queries.GetInvoiceAuditlog
{
    public class GetInvoiceAuditlogQueryResult : BasePaginatedResult
    {
        public Guid InvoiceId { get; set; }
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}