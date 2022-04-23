using AutoMapper;
using Demo.Application.UserPreferences.Queries.GetUserPreferences.Dtos;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferences
{
    public class GetUserPreferencesMappingProfile : Profile
    {
        public GetUserPreferencesMappingProfile()
        {
            CreateMap<Domain.UserPreferences.UserPreferences, UserPreferencesDto>();
            CreateMap<Domain.UserPreferences.UserPreferencesPreferences, UserPreferencesPreferencesDto>();
        }
    }
}
