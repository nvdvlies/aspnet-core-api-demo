using Demo.Common.Interfaces;
using System;
using System.Globalization;

namespace Demo.WebApi.Services
{
    public class CurrentUserService : ICurrentUser
    {
        public Guid Id => Guid.NewGuid();

        public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // linux: Europe/Amsterdam

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture("nl-NL");
    }
}
