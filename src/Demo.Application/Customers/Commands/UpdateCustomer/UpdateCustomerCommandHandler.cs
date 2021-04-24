using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
    {
        private readonly ICustomerBusinessComponent _bc;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(
            ICustomerBusinessComponent bc, 
            IMapper mapper
        )
        {
            _bc = bc;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            _bc.MapFrom(request, _mapper);

            await _bc.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}