using System;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors
{
    internal class CommandNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var commandName = AnsiConsole.Ask<string>("What is the name of the command?");
            commandName = commandName.Replace("Command", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            context.Variables.Set(Constants.CommandName, commandName);
        }
    }
}