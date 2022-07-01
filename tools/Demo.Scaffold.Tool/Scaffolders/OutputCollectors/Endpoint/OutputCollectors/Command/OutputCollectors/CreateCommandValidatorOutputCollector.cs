using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors
{
    internal class CreateCommandValidatorOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);

            changes.Add(new CreateNewClass(
                context.GetCommandDirectory(controllerName, commandName),
                $"{commandName}CommandValidator.cs",
                GetTemplate(controllerName, commandName)
            ));

            return changes;
        }

        private static string GetTemplate(string controllerName, string commandName)
        {
            var code = @"using FluentValidation;

namespace Demo.Application.%CONTROLLERNAME%.Commands.%COMMANDNAME%
{
    public class %COMMANDNAME%CommandValidator : AbstractValidator<%COMMANDNAME%Command>
    {
        public %COMMANDNAME%CommandValidator()
        {
            // RuleFor(x => x.Id).NotEmpty();
        }
    }
}
";
            code = code.Replace("%CONTROLLERNAME%", controllerName);
            code = code.Replace("%COMMANDNAME%", commandName);
            return code;
        }
    }
}