using MediatR;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettingsAuditlog
{
    public class GetApplicationSettingsAuditlogQuery : IRequest<GetApplicationSettingsAuditlogQueryResult>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

    }
}