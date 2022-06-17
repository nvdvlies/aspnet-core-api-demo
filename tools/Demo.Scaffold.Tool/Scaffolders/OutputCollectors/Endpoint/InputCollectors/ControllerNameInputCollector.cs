using System;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.InputCollectors
{
    internal class ControllerNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var controllerName = AnsiConsole.Ask<string>("In which controller do you want to add an endpoint?");
            controllerName =
                controllerName.Replace("Controller", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            context.Variables.Set(Constants.ControllerName, controllerName);
        }
    }
}