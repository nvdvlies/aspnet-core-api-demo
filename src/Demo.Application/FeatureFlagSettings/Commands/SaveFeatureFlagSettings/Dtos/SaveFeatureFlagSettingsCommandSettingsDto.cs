using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings.Dtos;

public class SaveFeatureFlagSettingsCommandSettingsDto
{
    public List<SaveFeatureFlagSettingsCommandSettingsFeatureFlagDto> FeatureFlags { get; set; }
}
