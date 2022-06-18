using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Invoices.Queries.InvoiceLookup.Dtos;
using Demo.Application.Shared.Extensions;
using Demo.Application.Shared.Models;
using Demo.Domain.Invoice;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Invoices.Queries.InvoiceLookup
{
    public class InvoiceLookupQueryHandler : IRequestHandler<InvoiceLookupQuery, InvoiceLookupQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<Invoice> _query;

        public InvoiceLookupQueryHandler(
            IDbQuery<Invoice> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<InvoiceLookupQueryResult> Handle(InvoiceLookupQuery request,
            CancellationToken cancellationToken)
        {
            var query = request.Ids is { Length: > 0 }
                ? _query.WithOptions(x => x.IncludeDeleted = true).AsQueryable()
                : _query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.InvoiceNumber, $"%{request.SearchTerm}%")
                    || EF.Functions.Like(x.Customer.Name, $"%{request.SearchTerm}%"));
            }

            if (request.Ids is { Length: > 0 })
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var sortOrder = request.OrderByDescending ? SortDirection.Descending : SortDirection.Ascending;

            query = request.OrderBy switch
            {
                InvoiceLookupOrderByEnum.InvoiceNumber => query.OrderBy(x => x.InvoiceNumber, sortOrder),
                InvoiceLookupOrderByEnum.InvoiceDate => query.OrderBy(x => x.InvoiceDate, sortOrder),
                _ => throw new Exception($"OrderBy '{request.OrderBy}' not implemented.")
            };

            var invoices = await query
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ProjectTo<InvoiceLookupDto>(_mapper.ConfigurationProvider)
                //.WriteQueryStringToOutputWindowIfInDebugMode()
                .ToListAsync(cancellationToken);

            return new InvoiceLookupQueryResult
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Invoices = invoices
            };
        }
    }
}
