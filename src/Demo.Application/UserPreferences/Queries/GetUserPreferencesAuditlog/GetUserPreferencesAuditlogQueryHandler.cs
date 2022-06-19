using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Auditlog;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferencesAuditlog
{
    public class GetUserPreferencesAuditlogQueryHandler : IRequestHandler<GetUserPreferencesAuditlogQuery,
        GetUserPreferencesAuditlogQueryResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IDbQuery<Auditlog> _query;

        public GetUserPreferencesAuditlogQueryHandler(
            ICurrentUser currentUser,
            IDbQuery<Auditlog> query,
            IMapper mapper
        )
        {
            _currentUser = currentUser;
            _query = query;
            _mapper = mapper;
        }

        public async Task<GetUserPreferencesAuditlogQueryResult> Handle(GetUserPreferencesAuditlogQuery request,
            CancellationToken cancellationToken)
        {
            var query = _query.AsQueryable()
                .Include(x => x.AuditlogItems)
                .ThenInclude(y => y.AuditlogItems)
                .ThenInclude(y => y.AuditlogItems)
                .Where(x => x.EntityName == nameof(Domain.UserPreferences.UserPreferences))
                .Where(x => x.EntityId == _currentUser.Id);

            var totalItems = await query.CountAsync(cancellationToken);

            var auditLogs = await query
                .OrderByDescending(c => c.ModifiedOn)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new GetUserPreferencesAuditlogQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Auditlogs = _mapper.Map<IEnumerable<AuditlogDto>>(auditLogs)
            };
        }
    }
}
