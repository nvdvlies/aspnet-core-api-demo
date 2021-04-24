using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query;
using Demo.Scaffold.Tool.Interfaces;
using System;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors
{
    internal class EndpointTypeOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var endpointType = context.Variables.Get<EndpointTypes>(Constants.EndpointType);
            IOutputCollector scaffolder = endpointType switch
            {
                EndpointTypes.Command => new CommandOutputCollector(),
                EndpointTypes.Query => new QueryOutputCollector(),
                _ => throw new Exception($"Endpoint type '{endpointType}' is not supported"),
            };
            return scaffolder.CollectChanges(context);
        }
    }
}
