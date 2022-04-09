using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;
using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettingsAuditlog
{
    public class GetFeatureFlagSettingsAuditlogQueryResult : BasePaginatedResult
    {
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}