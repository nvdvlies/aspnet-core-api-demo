using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Shared.DomainEntity
{
    internal class PerformanceMeasurements
    {
        private readonly ILogger _logger;
        private List<PerformanceMeasurement> _performanceMeasurements;

        public PerformanceMeasurements(ILogger logger)
        {
            _logger = logger;
            Initialize();
        }

        private void Initialize()
        {
            _performanceMeasurements = new List<PerformanceMeasurement>();
        }

        public PerformanceMeasurement Start(string name)
        {
            var stopwatch = new PerformanceMeasurement(name);
            stopwatch.Start();
            _performanceMeasurements.Add(stopwatch);
            return stopwatch;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var stopwatch in _performanceMeasurements)
            {
                sb.AppendLine(stopwatch.ToString());
            }

            return sb.ToString();
        }

        public void Flush()
        {
            _logger.LogInformation(ToString());
            Initialize();
        }
    }
}
