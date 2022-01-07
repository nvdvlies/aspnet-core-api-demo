using Demo.Common.Interfaces;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Demo.WebApi.Services
{
    internal class CorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Id
        {
            get {
                var operation = _httpContextAccessor.HttpContext?.Features.Get<RequestTelemetry>()?.Context?.Operation;
                return !string.IsNullOrEmpty(operation.ParentId) ? operation.ParentId : operation.Id;
            }
        }
    }
}
