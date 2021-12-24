using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Customers.Queries.GetCustomerById.Dtos;
using Demo.Domain.Customer;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult>
    {
        private readonly IDbQuery<Customer> _query;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(
            IDbQuery<Customer> query,
            IMapper mapper
        )
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<GetCustomerByIdQueryResult> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _query.AsQueryable()
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return new GetCustomerByIdQueryResult
            {
                Customer = customer
            };
        }
    }
}