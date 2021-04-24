using AutoMapper;
using Demo.Application.ApplicationSettings.Queries.GetApplicationSettings.Dtos;

namespace Demo.Application.Shared.Mappings
{
    public class GetApplicationSettingsMappingProfile : Profile
    {
        public GetApplicationSettingsMappingProfile()
        {
            CreateMap<Domain.ApplicationSettings.ApplicationSettings, ApplicationSettingsDto>();
            CreateMap<Domain.ApplicationSettings.ApplicationSettingsSettings, ApplicationSettingsSettingsDto>();
        }
    }
}
