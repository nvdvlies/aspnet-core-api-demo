using Demo.Common.Interfaces;
using System;

namespace Demo.WebApi.Services
{
    internal class CorrelationIdProvider : ICorrelationIdProvider
    {
        public Guid Id => Guid.NewGuid(); // RequestTelemetry.Context.Operation.Id;
    }
}
