using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors
{
    internal class CommandEndpointTypeInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var commandEndpointTypes = (CommandEndpointTypes)Prompt.GetInt("Which type of endpoint do you want to use (1 = create, 2 = update, 3 = delete, 4 = create subendpoint, 5 = update subendpoint, 6 = delete subendpoint)?");
            context.Variables.Set(Constants.CommandEndpointType, commandEndpointTypes);
        }
    }
}
