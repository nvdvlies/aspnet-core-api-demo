using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;

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

        public Guid Id => Guid.Parse("3D223F4C-9578-4930-9034-830D1C923EF4"); // TODO: Guid.Parse(_httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // linux: Europe/Amsterdam

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture("nl-NL");
    }
}
