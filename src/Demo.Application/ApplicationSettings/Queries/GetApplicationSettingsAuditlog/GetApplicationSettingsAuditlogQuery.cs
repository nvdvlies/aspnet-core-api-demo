using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettingsAuditlog
{
    public class GetApplicationSettingsAuditlogQuery : IQuery, IRequest<GetApplicationSettingsAuditlogQueryResult>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

    }
}