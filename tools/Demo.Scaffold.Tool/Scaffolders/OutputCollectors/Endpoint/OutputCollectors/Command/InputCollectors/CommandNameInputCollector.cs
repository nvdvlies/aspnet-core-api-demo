using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors
{
    internal class CommandNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var commandName = Prompt.GetString("What is the name of the command?");
            commandName = commandName.Replace("Command", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            context.Variables.Set(Constants.CommandName, commandName);
        }
    }
}
