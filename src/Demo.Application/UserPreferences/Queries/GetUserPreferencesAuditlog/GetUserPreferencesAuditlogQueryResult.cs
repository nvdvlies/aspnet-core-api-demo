using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferencesAuditlog
{
    public class GetUserPreferencesAuditlogQueryResult : BasePaginatedResult
    {
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}