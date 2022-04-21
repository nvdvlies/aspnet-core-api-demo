using Demo.Domain.Shared.Entities;
using System.Text.Json.Serialization;

namespace Demo.Domain.FeatureFlagSettings
{
    public partial class FeatureFlagSettings : AuditableEntity
    {
        public FeatureFlagSettings()
        {
            Settings = new FeatureFlagSettingsSettings();
        }

        [JsonInclude]
        public FeatureFlagSettingsSettings Settings { get; internal set; }
    }
}