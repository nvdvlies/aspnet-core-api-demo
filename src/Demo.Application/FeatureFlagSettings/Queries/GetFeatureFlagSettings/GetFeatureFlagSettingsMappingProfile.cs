using AutoMapper;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings
{
    public class GetFeatureFlagSettingsMappingProfile : Profile
    {
        public GetFeatureFlagSettingsMappingProfile()
        {
            CreateMap<Domain.FeatureFlagSettings.FeatureFlagSettings, FeatureFlagSettingsDto>();
            CreateMap<Domain.FeatureFlagSettings.FeatureFlagSettingsSettings, FeatureFlagSettingsSettingsDto>();
            CreateMap<Domain.FeatureFlagSettings.FeatureFlag, FeatureFlagDto>();
        }
    }
}
