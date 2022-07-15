using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors
{
    internal class AddUsingStatementToControllerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var queryName = context.Variables.Get<string>(Constants.QueryName);

            changes.Add(new AddUsingStatementToExistingClass(
                context.GetControllersDirectory(),
                context.GetControllerFileName(controllerName),
                $"using Demo.Application.{controllerName}.Queries.{queryName};"
            ));

            return changes;
        }
    }
}
