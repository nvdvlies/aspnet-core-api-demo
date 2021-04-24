using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly ICustomerBusinessComponent _bc;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(
            ICustomerBusinessComponent bc, 
            IMapper mapper
        )
        {
            _bc = bc;
            _mapper = mapper;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _bc.NewAsync(cancellationToken);

            _bc.MapFrom(request, _mapper);

            await _bc.CreateAsync(cancellationToken);

            return new CreateCustomerResponse
            {
                Id = _bc.EntityId
            };
        }
    }
}