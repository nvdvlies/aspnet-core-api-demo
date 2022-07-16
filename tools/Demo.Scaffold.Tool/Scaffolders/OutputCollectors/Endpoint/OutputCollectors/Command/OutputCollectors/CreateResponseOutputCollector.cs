using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors;

internal class CreateResponseOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var commandEndpointType = context.Variables.Get<CommandEndpointTypes>(Constants.CommandEndpointType);
        var hasResponse = commandEndpointType == CommandEndpointTypes.Create;

        if (hasResponse)
        {
            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);

            changes.Add(new CreateNewClass(
                context.GetCommandDirectory(controllerName, commandName),
                $"{commandName}Response.cs",
                GetTemplate(controllerName, commandName)
            ));
        }

        return changes;
    }

    private static string GetTemplate(string controllerName, string commandName)
    {
        var code = @"
using System;

namespace Demo.Application.%CONTROLLERNAME%.Commands.%COMMANDNAME%
{
    public class %COMMANDNAME%Response
    {
        public Guid Id { get; set; }
    }
}
";
        code = code.Replace("%CONTROLLERNAME%", controllerName);
        code = code.Replace("%COMMANDNAME%", commandName);
        return code;
    }
}
