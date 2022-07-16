using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Auditlog;
using Demo.Domain.Customer;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Customers.Queries.GetCustomerAuditlog;

public class
    GetCustomerAuditlogQueryHandler : IRequestHandler<GetCustomerAuditlogQuery, GetCustomerAuditlogQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IDbQuery<Auditlog> _query;

    public GetCustomerAuditlogQueryHandler(
        IDbQuery<Auditlog> query,
        IMapper mapper
    )
    {
        _query = query;
        _mapper = mapper;
    }

    public async Task<GetCustomerAuditlogQueryResult> Handle(GetCustomerAuditlogQuery request,
        CancellationToken cancellationToken)
    {
        var query = _query.AsQueryable()
            .Where(x => x.EntityName == nameof(Customer))
            .Where(x => x.EntityId == request.CustomerId);

        var totalItems = await query.CountAsync(cancellationToken);

        var auditLogs = await query
            .OrderByDescending(c => c.ModifiedOn)
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetCustomerAuditlogQueryResult
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            CustomerId = request.CustomerId,
            Auditlogs = _mapper.Map<IEnumerable<AuditlogDto>>(auditLogs)
        };
    }
}
