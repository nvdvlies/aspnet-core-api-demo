using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Demo.WebApi.Middleware
{
    public class CurrentUserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IHttpContextAccessor httpContextAccessor,
            ICurrentUserIdProvider currentUserIdProvider)
        {
            var externalId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            currentUserIdProvider.SetUserId(externalId);

            await _next.Invoke(httpContext);
        }
    }
}