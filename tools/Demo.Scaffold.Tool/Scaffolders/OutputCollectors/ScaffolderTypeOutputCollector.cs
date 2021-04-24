using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint;
using Demo.Scaffold.Tool.Interfaces;
using System;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors
{
    internal class ScaffolderTypeOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            IOutputCollector scaffolder = context.ScaffolderType switch
            {
                ScaffolderTypes.BusinessComponent => new BusinessComponentOutputCollector(),
                ScaffolderTypes.Endpoint => new EndpointOutputCollector(),
                _ => throw new Exception($"Scaffolder type '{context.ScaffolderType}' is not supported"),
            };
            return scaffolder.CollectChanges(context);
        }
    }
}
