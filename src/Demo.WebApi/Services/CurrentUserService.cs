using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace Demo.WebApi.Services
{
    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            IUserIdProvider userIdProvider)
        {
            ExternalId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Id = userIdProvider.Get(ExternalId);
        }

        public Guid Id { get; }

        public string ExternalId { get; }

        public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // linux: Europe/Amsterdam

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture("nl-NL");
    }
}
