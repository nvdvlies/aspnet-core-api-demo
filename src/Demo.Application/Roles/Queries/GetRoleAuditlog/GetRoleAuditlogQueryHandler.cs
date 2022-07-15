using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Auditlog;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Roles.Queries.GetRoleAuditlog
{
    public class GetRoleAuditlogQueryHandler : IRequestHandler<GetRoleAuditlogQuery, GetRoleAuditlogQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<Auditlog> _query;

        public GetRoleAuditlogQueryHandler(
            IDbQuery<Auditlog> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<GetRoleAuditlogQueryResult> Handle(GetRoleAuditlogQuery request,
            CancellationToken cancellationToken)
        {
            var query = _query.AsQueryable()
                .Where(x => x.EntityName == nameof(Role))
                .Where(x => x.EntityId == request.RoleId);

            var totalItems = await query.CountAsync(cancellationToken);

            var auditLogs = await query
                .OrderByDescending(c => c.ModifiedOn)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new GetRoleAuditlogQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                RoleId = request.RoleId,
                Auditlogs = _mapper.Map<IEnumerable<AuditlogDto>>(auditLogs)
            };
        }
    }
}
