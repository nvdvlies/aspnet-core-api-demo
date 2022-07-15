using System.Collections.Generic;
using System.Linq;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query
{
    internal class QueryOutputCollector : IOutputCollector
    {
        private readonly List<IInputCollector> _inputCollectors = new()
        {
            new QueryNameInputCollector(), new QueryEndpointTypeInputCollector()
        };

        private readonly List<IOutputCollector> _outputCollectors = new()
        {
            new CreateControllerIfNotExistsOutputCollector(),
            new AddUsingStatementToControllerOutputCollector(),
            new AddEndpointToControllerOutputCollector(),
            new CreateQueryOutputCollector(),
            new CreateQueryResultOutputCollector(),
            new CreateQueryHandlerOutputCollector()
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
