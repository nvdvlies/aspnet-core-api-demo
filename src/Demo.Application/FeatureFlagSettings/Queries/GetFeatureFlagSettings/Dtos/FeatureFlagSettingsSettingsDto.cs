using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos
{
    public class FeatureFlagSettingsSettingsDto
    {
        public List<FeatureFlagDto> FeatureFlags { get; set; }
    }
}