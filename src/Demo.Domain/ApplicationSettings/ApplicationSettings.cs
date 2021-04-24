using Demo.Domain.Shared.Entities;

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

        public ApplicationSettingsSettings Settings { get; internal set; }
    }
}
