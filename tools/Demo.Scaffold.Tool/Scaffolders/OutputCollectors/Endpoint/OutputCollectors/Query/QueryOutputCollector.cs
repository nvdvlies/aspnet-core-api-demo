using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query
{
    internal class QueryOutputCollector : IOutputCollector
    {
        private List<IInputCollector> _inputCollectors = new List<IInputCollector>()
        {
            new QueryNameInputCollector(),
            new QueryEndpointTypeInputCollector()
        };

        private List<IOutputCollector> _outputCollectors = new List<IOutputCollector>()
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
