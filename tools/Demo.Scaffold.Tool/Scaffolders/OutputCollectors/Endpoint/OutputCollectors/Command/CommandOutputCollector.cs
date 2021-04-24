using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command
{
    internal class CommandOutputCollector : IOutputCollector
    {
        private List<IInputCollector> _inputCollectors = new List<IInputCollector>()
        {
            new CommandEndpointTypeInputCollector(),
            new CommandNameInputCollector(),
        };

        private List<IOutputCollector> _outputCollectors = new List<IOutputCollector>()
        {
            new CreateControllerIfNotExistsOutputCollector(),
            new AddUsingStatementToControllerOutputCollector(),
            new AddEndpointToControllerOutputCollector(),
            new CreateCommandOutputCollector(),
            new CreateResponseOutputCollector(),
            new CreateCommandHandlerOutputCollector(),
            new CreateCommandValidatorOutputCollector()
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
