using AutoMapper;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos;
using Demo.Domain.FeatureFlagSettings;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings
{
    public class GetFeatureFlagSettingsMappingProfile : Profile
    {
        public GetFeatureFlagSettingsMappingProfile()
        {
            CreateMap<Domain.FeatureFlagSettings.FeatureFlagSettings, FeatureFlagSettingsDto>();
            CreateMap<FeatureFlagSettingsSettings, FeatureFlagSettingsSettingsDto>();
            CreateMap<FeatureFlag, FeatureFlagDto>();
        }
    }
}
