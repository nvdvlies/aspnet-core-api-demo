using AutoMapper;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings
{
    public class GetFeatureFlagSettingsQueryHandler : IRequestHandler<GetFeatureFlagSettingsQuery, GetFeatureFlagSettingsQueryResult>
    {
        private readonly IFeatureFlagSettingsProvider _featureFlagSettingsProvider;
        private readonly IMapper _mapper;

        public GetFeatureFlagSettingsQueryHandler(
            IFeatureFlagSettingsProvider featureFlagSettingsProvider,
            IMapper mapper
        )
        {
            _featureFlagSettingsProvider = featureFlagSettingsProvider;
            _mapper = mapper;
        }

        public async Task<GetFeatureFlagSettingsQueryResult> Handle(GetFeatureFlagSettingsQuery request, CancellationToken cancellationToken)
        {
            var FeatureFlagSettings = await _featureFlagSettingsProvider.GetAsync(cancellationToken);

            var FeatureFlagSettingsDto = _mapper.Map<FeatureFlagSettingsDto>(FeatureFlagSettings);

            return new GetFeatureFlagSettingsQueryResult
            {
                FeatureFlagSettings = FeatureFlagSettingsDto
            };
        }
    }
}