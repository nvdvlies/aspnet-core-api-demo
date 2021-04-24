using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.ApplicationSettings.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommandHandler : IRequestHandler<SaveApplicationSettingsCommand, Unit>
    {
        private readonly IApplicationSettingsBusinessComponent _bc;
        private readonly IMapper _mapper;

        public SaveApplicationSettingsCommandHandler(
            IApplicationSettingsBusinessComponent bc,
            IMapper mapper
        )
        {
            _bc = bc;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveApplicationSettingsCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(cancellationToken);

            _bc.MapFrom(request, _mapper);

            await _bc.UpsertAsync(cancellationToken);

            return Unit.Value;
        }
    }
}