using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;

namespace Demo.WebApi.Services
{
    public class CurrentUserService : ICurrentUser
    {
        private readonly EnvironmentSettings _environmentSettings;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            IUserIdProvider userIdProvider,
            EnvironmentSettings environmentSettings)
        {
            _environmentSettings = environmentSettings;
            ExternalId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Id = userIdProvider.Get(ExternalId);
        }

        public Guid Id { get; }

        public string ExternalId { get; }

        public TimeZoneInfo TimeZone =>
            TimeZoneInfo.FindSystemTimeZoneById(_environmentSettings.DefaultTimeZone ?? "W. Europe Standard Time");

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture("nl-NL");
    }
}
