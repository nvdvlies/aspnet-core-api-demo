using Demo.Common.Interfaces;
using System;

namespace Demo.Infrastructure.Services
{
    internal class CorrelationIdProvider : ICorrelationIdProvider
    {
        public string Id => Guid.NewGuid().ToString(); // RequestTelemetry.Context.Operation.Id;
    }
}
