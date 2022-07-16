using AutoMapper;
using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings.Dtos;
using Demo.Domain.ApplicationSettings;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings;

public class SaveApplicationSettingsMappingProfile : Profile
{
    public SaveApplicationSettingsMappingProfile()
    {
        CreateMap<SaveApplicationSettingsCommand, Domain.ApplicationSettings.ApplicationSettings>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.CreatedOn, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedOn, opt => opt.Ignore());

        CreateMap<SaveApplicationSettingsSettingsDto, ApplicationSettingsSettings>();
    }
}
