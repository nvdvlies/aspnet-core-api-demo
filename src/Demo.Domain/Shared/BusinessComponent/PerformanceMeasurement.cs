using System.Diagnostics;

namespace Demo.Domain.Shared.BusinessComponent
{
    internal class PerformanceMeasurement
    {
        private readonly string _name;
        private Stopwatch _stopwatch { get; set; }

        public PerformanceMeasurement(string name)
        {
            _name = name;
            _stopwatch = new Stopwatch();
        }

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
