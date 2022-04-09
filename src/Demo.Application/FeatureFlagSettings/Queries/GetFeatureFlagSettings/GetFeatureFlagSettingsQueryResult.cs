using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings
{
    public class GetFeatureFlagSettingsQueryResult
    {
        public FeatureFlagSettingsDto FeatureFlagSettings { get; set; }
    }
}