using System;
using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Customers.Queries.GetCustomerAuditlog
{
    public class GetCustomerAuditlogQueryResult : BasePaginatedResult
    {
        public Guid CustomerId { get; set; }
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}
