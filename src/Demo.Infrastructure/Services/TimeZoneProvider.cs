using System;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences.Interfaces;
using Demo.Infrastructure.Settings;

namespace Demo.Infrastructure.Services
{
    internal class TimeZoneProvider : ITimeZoneProvider
    {
        public TimeZoneProvider(
            EnvironmentSettings environmentSettings,
            IApplicationSettingsProvider applicationSettingsProvider,
            IUserPreferencesProvider userPreferencesProvider
        )
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(environmentSettings.DefaultTimeZone ??
                                                           "W. Europe Standard Time"); // TODO: UserPreferences ?? ApplicationSettings ?? EnvironmentSettings
        }

        public TimeZoneInfo TimeZone { get; }
    }
}
