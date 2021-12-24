using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors
{
    internal class CreateCommandOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var commandEndpointType = context.Variables.Get<CommandEndpointTypes>(Constants.CommandEndpointType);
            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);
            var entityName = controllerName.EndsWith("s") ? controllerName[0..^1] : controllerName;

            changes.Add(new CreateNewClass(
                    directory: context.GetCommandDirectory(controllerName, commandName),
                    fileName: $"{commandName}Command.cs",
                    content: GetTemplate(commandEndpointType, controllerName, commandName, entityName)
                ));

            return changes;
        }

        private static string GetTemplate(CommandEndpointTypes commandEndpointType, string controllerName, string commandName, string entityName)
        {
            var code = commandEndpointType == CommandEndpointTypes.CreateSubEndpoint
                    || commandEndpointType == CommandEndpointTypes.UpdateSubEndpoint
                    || commandEndpointType == CommandEndpointTypes.DeleteSubEndpoint
                    || commandEndpointType == CommandEndpointTypes.Update
                    || commandEndpointType == CommandEndpointTypes.Delete ? @"
using MediatR;
using System;

namespace Demo.Application.%CONTROLLERNAME%.Commands.%COMMANDNAME%
{
    public class %COMMANDNAME%Command : IRequest<%RESPONSE%>
    {
        internal Guid Id { get; set; }

        public void Set%ENTITYNAME%Id(Guid id)
        {
            Id = id;
        }
    }
}
" :
@"
using MediatR;
using System;

namespace Demo.Application.%CONTROLLERNAME%.Commands.%COMMANDNAME%
{
    public class %COMMANDNAME%Command : IRequest<%RESPONSE%>
    {
    }
}
";
            var hasResponse = commandEndpointType == CommandEndpointTypes.Create;
            code = code.Replace("%RESPONSE%", !hasResponse ? "Unit" : $"{commandName}Response");
            code = code.Replace("%CONTROLLERNAME%", controllerName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENTITYNAME%", entityName);
            return code;
        }
    }
}
