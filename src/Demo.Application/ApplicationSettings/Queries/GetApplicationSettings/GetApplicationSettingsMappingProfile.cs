using AutoMapper;
using Demo.Application.ApplicationSettings.Queries.GetApplicationSettings.Dtos;
using Demo.Domain.ApplicationSettings;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettings
{
    public class GetApplicationSettingsMappingProfile : Profile
    {
        public GetApplicationSettingsMappingProfile()
        {
            CreateMap<Domain.ApplicationSettings.ApplicationSettings, ApplicationSettingsDto>();
            CreateMap<ApplicationSettingsSettings, ApplicationSettingsSettingsDto>();
        }
    }
}