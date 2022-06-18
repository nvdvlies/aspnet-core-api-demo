using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Invoices.Queries.GetInvoiceById.Dtos;
using Demo.Domain.Invoice;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, GetInvoiceByIdQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IDbQuery<Invoice> _query;

        public GetInvoiceByIdQueryHandler(
            IDbQuery<Invoice> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<GetInvoiceByIdQueryResult> Handle(GetInvoiceByIdQuery request,
            CancellationToken cancellationToken)
        {
            var invoice = await _query.AsQueryable()
                .Include(invoice => invoice.InvoiceLines)
                .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (invoice != null)
            {
                invoice.InvoiceLines = invoice.InvoiceLines.OrderBy(x => x.LineNumber).ToList();
            }

            return new GetInvoiceByIdQueryResult { Invoice = invoice };
        }
    }
}
