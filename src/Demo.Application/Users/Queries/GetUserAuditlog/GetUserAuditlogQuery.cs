using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Users.Queries.GetUserAuditlog
{
    public class GetUserAuditlogQuery : IQuery, IRequest<GetUserAuditlogQueryResult>
    {
        internal Guid UserId { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public void SetUserId(Guid id)
        {
            UserId = id;
        }
    }
}
