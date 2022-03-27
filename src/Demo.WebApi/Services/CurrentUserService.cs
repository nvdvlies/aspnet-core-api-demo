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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id
        {
            get
            {
                var auth0UserId = _httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var auth0UserIdWithoutPrefix = auth0UserId.Substring(auth0UserId.LastIndexOf('|') + 1);
                Guid.TryParse(auth0UserIdWithoutPrefix, out var id);
                return id;
            }
        }

        public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // linux: Europe/Amsterdam

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture("nl-NL");
    }
}
