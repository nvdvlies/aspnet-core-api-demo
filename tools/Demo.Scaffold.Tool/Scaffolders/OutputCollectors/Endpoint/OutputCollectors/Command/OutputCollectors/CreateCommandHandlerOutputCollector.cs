using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors
{
    internal class CreateCommandHandlerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);
            var commandEndpointType = context.Variables.Get<CommandEndpointTypes>(Constants.CommandEndpointType);

            changes.Add(new CreateNewClass(
                context.GetCommandDirectory(controllerName, commandName),
                $"{commandName}CommandHandler.cs",
                GetTemplate(controllerName, commandName, commandEndpointType)
            ));

            return changes;
        }

        private static string GetTemplate(string controllerName, string commandName,
            CommandEndpointTypes commandEndpointType)
        {
            var code = @"
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.%CONTROLLERNAME%.Commands.%COMMANDNAME%
{
    public class %COMMANDNAME%CommandHandler : IRequestHandler<%COMMANDNAME%Command, %RESPONSE%>
    {
        public %COMMANDNAME%CommandHandler()
        {
        }

        public async Task<%RESPONSE%> Handle(%COMMANDNAME%Command request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return %RETURN%;
        }
    }
}
";
            var hasResponse = commandEndpointType == CommandEndpointTypes.Create;
            code = code.Replace("%RESPONSE%", !hasResponse ? "Unit" : $"{commandName}Response");
            code = code.Replace("%RETURN%", !hasResponse ? "Unit.Value" : $"new {commandName}Response()");
            code = code.Replace("%CONTROLLERNAME%", controllerName);
            code = code.Replace("%COMMANDNAME%", commandName);
            return code;
        }
    }
}
