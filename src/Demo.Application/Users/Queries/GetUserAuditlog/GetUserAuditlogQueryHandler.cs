using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Auditlog;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Users.Queries.GetUserAuditlog
{
    public class GetUserAuditlogQueryHandler : IRequestHandler<GetUserAuditlogQuery, GetUserAuditlogQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<Auditlog> _query;

        public GetUserAuditlogQueryHandler(
            IDbQuery<Auditlog> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<GetUserAuditlogQueryResult> Handle(GetUserAuditlogQuery request,
            CancellationToken cancellationToken)
        {
            var query = _query.AsQueryable()
                .Where(x => x.EntityName == nameof(User))
                .Where(x => x.EntityId == request.UserId);

            var totalItems = await query.CountAsync(cancellationToken);

            var auditLogs = await query
                .OrderByDescending(c => c.ModifiedOn)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new GetUserAuditlogQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                UserId = request.UserId,
                Auditlogs = _mapper.Map<IEnumerable<AuditlogDto>>(auditLogs)
            };
        }
    }
}