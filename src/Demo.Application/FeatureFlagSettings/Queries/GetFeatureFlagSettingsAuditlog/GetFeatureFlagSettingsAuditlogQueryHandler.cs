using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Common.Extensions;
using Demo.Domain.Auditlog;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettingsAuditlog;

public class GetFeatureFlagSettingsAuditlogQueryHandler : IRequestHandler<GetFeatureFlagSettingsAuditlogQuery,
    GetFeatureFlagSettingsAuditlogQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IDbQuery<Auditlog> _query;

    public GetFeatureFlagSettingsAuditlogQueryHandler(
        IDbQuery<Auditlog> query,
        IMapper mapper
    )
    {
        _query = query;
        _mapper = mapper;
    }

    public async Task<GetFeatureFlagSettingsAuditlogQueryResult> Handle(GetFeatureFlagSettingsAuditlogQuery request,
        CancellationToken cancellationToken)
    {
        var encodedName = JsonEncodedText.Encode(request.Name);
        var query = _query.AsQueryable()
            .Where(x => x.EntityName == nameof(Domain.FeatureFlagSettings.FeatureFlagSettings))
            .Where(x => EF.Functions.JsonContains(x.AuditlogItems,
                @$"[ {{ ""{nameof(Auditlog.AuditlogItems)}"": [ {{ ""{nameof(AuditlogItem.PropertyName)}"": ""{encodedName}"" }} ] }} ]"));

        var totalItems = await query.CountAsync(cancellationToken);

        var auditLogs = await query
            .OrderByDescending(c => c.ModifiedOn)
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .WriteQueryStringToOutputWindowIfInDebugMode()
            .ToListAsync(cancellationToken);

        return new GetFeatureFlagSettingsAuditlogQueryResult
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            Auditlogs = _mapper.Map<IEnumerable<AuditlogDto>>(auditLogs)
        };
    }
}
