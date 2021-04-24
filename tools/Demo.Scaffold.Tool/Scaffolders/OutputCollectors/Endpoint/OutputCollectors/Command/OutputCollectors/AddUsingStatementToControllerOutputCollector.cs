using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors
{
    internal class AddUsingStatementToControllerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);

            changes.Add(new AddUsingStatementToExistingClass(
                directory: context.GetControllersDirectory(),
                fileName: context.GetControllerFileName(controllerName),
                content: $"using Demo.Application.{controllerName}.Commands.{commandName};"
            ));

            return changes;
        }
    }
}
