using Demo.Domain.Shared.Entities;
using System.Collections.Generic;

namespace Demo.Domain.FeatureFlagSettings
{
    public partial class FeatureFlagSettings : AuditableEntity
    {
        public FeatureFlagSettings()
        {
            FeatureFlags = new List<FeatureFlag>();
        }

        public List<FeatureFlag> FeatureFlags { get; set; }
    }
}