using System.Diagnostics;

namespace Demo.Domain.Shared.DomainEntity
{
    internal class PerformanceMeasurement
    {
        private readonly string _name;

        public PerformanceMeasurement(string name)
        {
            _name = name;
            _stopwatch = new Stopwatch();
        }

        private Stopwatch _stopwatch { get; }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public override string ToString()
        {
            return $"{_name}: {_stopwatch.Elapsed.TotalMilliseconds}ms";
        }
    }
}
