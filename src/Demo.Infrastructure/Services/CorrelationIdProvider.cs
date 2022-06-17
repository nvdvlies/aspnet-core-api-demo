using System;
using Demo.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Services
{
    internal class CorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly ILogger<CorrelationIdProvider> _logger;

        public CorrelationIdProvider(ILogger<CorrelationIdProvider> logger)
        {
            _logger = logger;
        }

        public Guid Id { get; private set; }

        public void SwitchToCorrelationId(Guid id)
        {
            if (Id != Guid.Empty && Id != id)
            {
                _logger.LogInformation("Switching correlation id from '{0}' to '{1}'", Id, id);
            }

            Id = id;
        }
    }
}