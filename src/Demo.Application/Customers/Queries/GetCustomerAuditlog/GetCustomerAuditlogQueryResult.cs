using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;
using System;
using System.Collections.Generic;

namespace Demo.Application.Customers.Queries.GetCustomerAuditlog
{
    public class GetCustomerAuditlogQueryResult : BasePaginatedResult
    {
        public Guid CustomerId { get; set; }
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}