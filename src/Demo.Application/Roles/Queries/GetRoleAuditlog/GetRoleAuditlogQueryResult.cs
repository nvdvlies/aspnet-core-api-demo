using System;
using System.Collections.Generic;
using Demo.Application.Shared.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Roles.Queries.GetRoleAuditlog;

public class GetRoleAuditlogQueryResult : BasePaginatedResult
{
    public Guid RoleId { get; set; }
    public IEnumerable<AuditlogDto> Auditlogs { get; set; }
}
