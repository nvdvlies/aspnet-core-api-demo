using Demo.Application.Shared.Dtos;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferences.Dtos;

public class UserPreferencesDto : AuditableEntityDto
{
    public UserPreferencesPreferencesDto Preferences { get; set; }
}
