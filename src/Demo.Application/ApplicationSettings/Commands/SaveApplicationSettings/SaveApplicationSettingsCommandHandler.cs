using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.ApplicationSettings.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommandHandler : IRequestHandler<SaveApplicationSettingsCommand, Unit>
    {
        private readonly IApplicationSettingsDomainEntity _applicationSettingsDomainEntity;
        private readonly IMapper _mapper;

        public SaveApplicationSettingsCommandHandler(
            IApplicationSettingsDomainEntity applicationSettingsDomainEntity,
            IMapper mapper
        )
        {
            _applicationSettingsDomainEntity = applicationSettingsDomainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveApplicationSettingsCommand request, CancellationToken cancellationToken)
        {
            await _applicationSettingsDomainEntity.GetAsync(cancellationToken);

            _applicationSettingsDomainEntity.MapFrom(request, _mapper);

            await _applicationSettingsDomainEntity.UpsertAsync(cancellationToken);

            return Unit.Value;
        }
    }
}