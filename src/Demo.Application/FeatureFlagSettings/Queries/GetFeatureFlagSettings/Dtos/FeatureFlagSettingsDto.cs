using Demo.Application.Shared.Dtos;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos;

public class FeatureFlagSettingsDto : AuditableEntityDto
{
    public FeatureFlagSettingsSettingsDto Settings { get; set; }
}
