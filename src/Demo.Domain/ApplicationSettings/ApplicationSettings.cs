using Demo.Domain.Shared.Entities;
using System.Text.Json.Serialization;

namespace Demo.Domain.ApplicationSettings
{
    public partial class ApplicationSettings : AuditableEntity
    {
        public ApplicationSettings()
        {
            Settings = new ApplicationSettingsSettings
            {
                Setting1 = true
            };
        }

        [JsonInclude]
        public ApplicationSettingsSettings Settings { get; internal set; }
    }
}
