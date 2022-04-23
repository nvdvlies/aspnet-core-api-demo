using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferencesAuditlog
{
    public class GetUserPreferencesAuditlogQuery : IQuery, IRequest<GetUserPreferencesAuditlogQueryResult>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}