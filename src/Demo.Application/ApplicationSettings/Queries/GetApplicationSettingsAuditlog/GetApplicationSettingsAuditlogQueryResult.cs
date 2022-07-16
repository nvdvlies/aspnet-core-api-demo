using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettingsAuditlog;

public class GetApplicationSettingsAuditlogQueryResult : BasePaginatedResult
{
    public IEnumerable<AuditlogDto> Auditlogs { get; set; }
}
