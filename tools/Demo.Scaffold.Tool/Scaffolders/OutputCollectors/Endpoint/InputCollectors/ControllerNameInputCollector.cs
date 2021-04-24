using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.InputCollectors
{
    internal class ControllerNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var controllerName = Prompt.GetString("In which controller do you want to add an endpoint?");
            controllerName = controllerName.Replace("Controller", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            context.Variables.Set(Constants.ControllerName, controllerName);
        }
    }
}
