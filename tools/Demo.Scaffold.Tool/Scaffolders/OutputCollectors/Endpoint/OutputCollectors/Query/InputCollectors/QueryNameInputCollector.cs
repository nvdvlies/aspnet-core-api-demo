using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors
{
    internal class QueryNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var queryName = Prompt.GetString("What is the name of the query?");
            queryName = queryName.Replace("Query", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            context.Variables.Set(Constants.QueryName, queryName);
        }
    }
}
