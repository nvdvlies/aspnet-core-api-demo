using AutoMapper;
using Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings.Dtos;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings
{
    public class SaveFeatureFlagSettingsMappingProfile : Profile
    {
        public SaveFeatureFlagSettingsMappingProfile()
        {
            CreateMap<SaveFeatureFlagSettingsCommand, Domain.FeatureFlagSettings.FeatureFlagSettings>()
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<SaveFeatureFlagSettingsCommandSettingsDto, Domain.FeatureFlagSettings.FeatureFlagSettingsSettings>();
            
            CreateMap<SaveFeatureFlagSettingsCommandSettingsFeatureFlagDto, Domain.FeatureFlagSettings.FeatureFlag>()
                .ForMember(x => x.Timestamp, opt => opt.Ignore());
        }
    }
}