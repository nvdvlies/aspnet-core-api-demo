using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using MediatR;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings
{
    public class SaveFeatureFlagSettingsCommandHandler : IRequestHandler<SaveFeatureFlagSettingsCommand, Unit>
    {
        private readonly IFeatureFlagSettingsDomainEntity _featureFlagSettingsDomainEntity;
        private readonly IMapper _mapper;

        public SaveFeatureFlagSettingsCommandHandler(
            IFeatureFlagSettingsDomainEntity featureFlagSettingsDomainEntity,
            IMapper mapper
        )
        {
            _featureFlagSettingsDomainEntity = featureFlagSettingsDomainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveFeatureFlagSettingsCommand request, CancellationToken cancellationToken)
        {
            await _featureFlagSettingsDomainEntity.GetAsync(cancellationToken);

            _featureFlagSettingsDomainEntity.MapFrom(request, _mapper);

            await _featureFlagSettingsDomainEntity.UpsertAsync(cancellationToken);

            return Unit.Value;
        }
    }
}