using System.Collections.Generic;
using System.Linq;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint
{
    internal class EndpointOutputCollector : IOutputCollector
    {
        private readonly List<IInputCollector> _inputCollectors = new()
        {
            new CommandOrQueryInputCollector(), new ControllerNameInputCollector()
        };

        private readonly List<IOutputCollector> _outputCollectors = new() { new EndpointTypeOutputCollector() };

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