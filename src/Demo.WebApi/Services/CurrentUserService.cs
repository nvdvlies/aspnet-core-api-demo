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
        private readonly Guid _id;
        private readonly string _externalId;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            IUserIdProvider userIdProvider)
        {
            _externalId = httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            _id = userIdProvider.Get(ExternalId);
        }

        public Guid Id => _id;

        public string ExternalId => _externalId;

        public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // linux: Europe/Amsterdam

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture("nl-NL");
    }
}
