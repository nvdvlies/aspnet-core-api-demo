using System;
using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Users.Queries.GetUserAuditlog
{
    public class GetUserAuditlogQueryResult : BasePaginatedResult
    {
        public Guid UserId { get; set; }
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}
