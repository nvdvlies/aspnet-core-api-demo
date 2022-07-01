using System.Text.Json.Serialization;
using Demo.Domain.Shared.Entities;

namespace Demo.Domain.FeatureFlagSettings
{
    public class FeatureFlagSettings : AuditableEntity
    {
        public FeatureFlagSettings()
        {
            Settings = new FeatureFlagSettingsSettings();
        }

        [JsonInclude] public FeatureFlagSettingsSettings Settings { get; internal set; }
    }
}