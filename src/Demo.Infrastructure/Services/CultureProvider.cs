using System.Globalization;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences.Interfaces;
using Demo.Infrastructure.Settings;

namespace Demo.Infrastructure.Services
{
    internal class CultureProvider : ICultureProvider
    {
        public CultureProvider(
            EnvironmentSettings environmentSettings,
            IApplicationSettingsProvider applicationSettingsProvider,
            IUserPreferencesProvider userPreferencesProvider
        )
        {
            Culture = CultureInfo
                .CreateSpecificCulture("nl-NL"); // TODO: UserPreferences ?? ApplicationSettings ?? EnvironmentSettings
        }

        public CultureInfo Culture { get; }
    }
}
