using System.Globalization;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Settings;

namespace Demo.Infrastructure.Services
{
    internal class CultureProvider : ICultureProvider
    {
        public CultureProvider(
            EnvironmentSettings environmentSettings
        )
        {
            Culture = CultureInfo.CreateSpecificCulture(environmentSettings.DefaultCulture ?? "nl-NL");
        }

        public CultureInfo Culture { get; }
    }
}