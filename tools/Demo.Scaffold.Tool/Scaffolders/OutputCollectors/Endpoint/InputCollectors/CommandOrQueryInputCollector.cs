using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.InputCollectors
{
    internal class CommandOrQueryInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var option = Prompt.GetInt("Endpoint type (1 = Command, 2 = Query):");

            switch (option)
            {
                case 1:
                    context.Variables.Set(Constants.EndpointType, EndpointTypes.Command);
                    break;
                case 2:
                    context.Variables.Set(Constants.EndpointType, EndpointTypes.Query);
                    break;
                default:
                    throw new Exception($"Invalid endpoint type '{option}'");
            }
        }
    }
}
