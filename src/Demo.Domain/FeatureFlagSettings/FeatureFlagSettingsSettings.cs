using System.Collections.Generic;

namespace Demo.Domain.FeatureFlagSettings
{
    public class FeatureFlagSettingsSettings
    {
        public FeatureFlagSettingsSettings()
        {
            FeatureFlags = new List<FeatureFlag>();
        }
        
        public List<FeatureFlag> FeatureFlags { get; set; }
    }
}