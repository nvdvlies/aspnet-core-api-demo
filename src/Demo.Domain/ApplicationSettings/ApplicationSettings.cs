using System.Text.Json.Serialization;
using Demo.Domain.Shared.Entities;

namespace Demo.Domain.ApplicationSettings
{
    public class ApplicationSettings : AuditableEntity
    {
        public ApplicationSettings()
        {
            Settings = new ApplicationSettingsSettings
            {
                Setting1 = true
            };
        }

        [JsonInclude] public ApplicationSettingsSettings Settings { get; internal set; }
    }
}