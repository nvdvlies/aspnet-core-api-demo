using Demo.Common.Interfaces;
using System;

namespace Demo.Infrastructure.Services
{
    internal class CorrelationIdProvider : ICorrelationIdProvider
    {
        public Guid Id => Guid.NewGuid(); // RequestTelemetry.Context.Operation.Id;
    }
}
