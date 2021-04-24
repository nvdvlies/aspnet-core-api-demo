using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors
{
    internal class QueryEndpointTypeInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var queryEndpointType = (QueryEndpointTypes)Prompt.GetInt("Which type of endpoint do you want to add (1:Search, 2:GetById, 3:SubEndpoint)?");
            context.Variables.Set(Constants.QueryEndpointType, queryEndpointType);
        }
    }
}
