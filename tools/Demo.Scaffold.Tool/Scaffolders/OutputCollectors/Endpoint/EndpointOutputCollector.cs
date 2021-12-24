using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint
{
    internal class EndpointOutputCollector : IOutputCollector
    {
        private List<IInputCollector> _inputCollectors = new List<IInputCollector>()
        {
            new CommandOrQueryInputCollector(),
            new ControllerNameInputCollector()
        };

        private List<IOutputCollector> _outputCollectors = new List<IOutputCollector>()
        {
            new EndpointTypeOutputCollector(),
        };

        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            foreach (var inputCollector in _inputCollectors)
            {
                inputCollector.CollectInput(context);
            }

            var changes = _outputCollectors
                .Select(x => x.CollectChanges(context))
                .SelectMany(x => x)
                .Where(x => x != null)
                .ToList();

            return changes;
        }
    }
}
