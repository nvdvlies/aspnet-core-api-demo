using System.Collections.Generic;
using System.Linq;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command
{
    internal class CommandOutputCollector : IOutputCollector
    {
        private readonly List<IInputCollector> _inputCollectors = new()
        {
            new CommandEndpointTypeInputCollector(), new CommandNameInputCollector()
        };

        private readonly List<IOutputCollector> _outputCollectors = new()
        {
            new CreateControllerIfNotExistsOutputCollector(),
            new AddUsingStatementToControllerOutputCollector(),
            new AddEndpointToControllerOutputCollector(),
            new CreateCommandOutputCollector(),
            new CreateResponseOutputCollector(),
            new CreateCommandHandlerOutputCollector(),
            new CreateCommandValidatorOutputCollector(),
            new CreateMappingProfileOutputCollector()
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
