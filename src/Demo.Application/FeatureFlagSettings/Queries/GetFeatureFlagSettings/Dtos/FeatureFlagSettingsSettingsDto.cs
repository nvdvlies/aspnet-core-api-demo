using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos;

public class FeatureFlagSettingsSettingsDto
{
    public List<FeatureFlagDto> FeatureFlags { get; set; }
}
