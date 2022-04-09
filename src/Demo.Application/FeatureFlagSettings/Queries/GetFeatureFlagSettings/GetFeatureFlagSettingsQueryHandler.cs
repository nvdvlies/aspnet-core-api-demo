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
        private readonly IFeatureFlagSettingsProvider _FeatureFlagSettingsProvider;
        private readonly IMapper _mapper;

        public GetFeatureFlagSettingsQueryHandler(
            IFeatureFlagSettingsProvider FeatureFlagSettingsProvider,
            IMapper mapper
        )
        {
            _FeatureFlagSettingsProvider = FeatureFlagSettingsProvider;
            _mapper = mapper;
        }

        public async Task<GetFeatureFlagSettingsQueryResult> Handle(GetFeatureFlagSettingsQuery request, CancellationToken cancellationToken)
        {
            var FeatureFlagSettings = await _FeatureFlagSettingsProvider.GetAsync(cancellationToken);

            var FeatureFlagSettingsDto = _mapper.Map<FeatureFlagSettingsDto>(FeatureFlagSettings);

            return new GetFeatureFlagSettingsQueryResult
            {
                FeatureFlagSettings = FeatureFlagSettingsDto
            };
        }
    }
}