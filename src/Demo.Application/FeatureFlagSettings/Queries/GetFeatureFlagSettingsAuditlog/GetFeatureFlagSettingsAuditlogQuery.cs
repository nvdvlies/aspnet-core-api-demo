using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettingsAuditlog
{
    public class GetFeatureFlagSettingsAuditlogQuery : IQuery, IRequest<GetFeatureFlagSettingsAuditlogQueryResult>
    {
        public string Name { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
