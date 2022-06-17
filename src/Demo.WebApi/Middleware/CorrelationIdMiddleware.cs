using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Demo.WebApi.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderKey = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ICorrelationIdProvider correlationIdProvider)
        {
            if (httpContext.Request.Headers.TryGetValue(HeaderKey, out var correlationIds))
            {
                var correlationIdString = correlationIds.FirstOrDefault(x => x.Equals(HeaderKey));

                if (!Guid.TryParse(correlationIdString, out var correlationId))
                {
                    correlationId = Guid.NewGuid();
                }

                correlationIdProvider.SwitchToCorrelationId(correlationId);
            }
            else
            {
                correlationIdProvider.SwitchToCorrelationId(Guid.NewGuid());
            }

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationIdProvider.Id))
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}