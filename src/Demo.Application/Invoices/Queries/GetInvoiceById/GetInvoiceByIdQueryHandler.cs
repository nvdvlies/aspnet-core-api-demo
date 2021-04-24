using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Invoices.Queries.GetInvoiceById.Dtos;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Invoice;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, GetInvoiceByIdQueryResult>
    {
        private readonly IDbQuery<Invoice> _query;
        private readonly IMapper _mapper;

        public GetInvoiceByIdQueryHandler(
            IDbQuery<Invoice> query, 
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<GetInvoiceByIdQueryResult> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _query.AsQueryable()
                .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return new GetInvoiceByIdQueryResult { Invoice = invoice };
        }
    }
}