using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Queries.GetRoleAuditlog
{
    public class GetRoleAuditlogQuery : IQuery, IRequest<GetRoleAuditlogQueryResult>
    {
        internal Guid RoleId { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public void SetRoleId(Guid id)
        {
            RoleId = id;
        }
    }
}
