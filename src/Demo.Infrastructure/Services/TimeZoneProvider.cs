using System;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Settings;

namespace Demo.Infrastructure.Services
{
    internal class TimeZoneProvider : ITimeZoneProvider
    {
        public TimeZoneProvider(
            EnvironmentSettings environmentSettings
        )
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(environmentSettings.DefaultTimeZone ??
                                                           "W. Europe Standard Time");
        }

        public TimeZoneInfo TimeZone { get; }
    }
}
