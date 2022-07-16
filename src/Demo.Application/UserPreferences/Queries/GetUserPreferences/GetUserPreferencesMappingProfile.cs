using AutoMapper;
using Demo.Application.UserPreferences.Queries.GetUserPreferences.Dtos;
using Demo.Domain.UserPreferences;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferences;

public class GetUserPreferencesMappingProfile : Profile
{
    public GetUserPreferencesMappingProfile()
    {
        CreateMap<Domain.UserPreferences.UserPreferences, UserPreferencesDto>();
        CreateMap<UserPreferencesPreferences, UserPreferencesPreferencesDto>();
    }
}
