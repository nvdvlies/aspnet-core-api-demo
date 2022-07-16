using System;
using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Invoices.Queries.GetInvoiceAuditlog;

public class GetInvoiceAuditlogQueryResult : BasePaginatedResult
{
    public Guid InvoiceId { get; set; }
    public IEnumerable<AuditlogDto> Auditlogs { get; set; }
}
