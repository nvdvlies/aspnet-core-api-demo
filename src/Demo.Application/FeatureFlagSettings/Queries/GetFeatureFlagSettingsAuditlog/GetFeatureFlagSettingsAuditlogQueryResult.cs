using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettingsAuditlog
{
    public class GetFeatureFlagSettingsAuditlogQueryResult : BasePaginatedResult
    {
        public IEnumerable<AuditlogDto> Auditlogs { get; set; }
    }
}