using AutoMapper;
using Demo.Application.UserPreferences.Commands.SaveUserPreferences.Dtos;
using Demo.Domain.UserPreferences;

namespace Demo.Application.UserPreferences.Commands.SaveUserPreferences
{
    public class SaveUserPreferencesMappingProfile : Profile
    {
        public SaveUserPreferencesMappingProfile()
        {
            CreateMap<SaveUserPreferencesCommand, Domain.UserPreferences.UserPreferences>()
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<SaveUserPreferencesPreferencesDto, UserPreferencesPreferences>();
        }
    }
}