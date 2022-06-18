using System;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors
{
    internal class QueryNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var queryName = AnsiConsole.Ask<string>("What is the name of the query?");
            queryName = queryName.Replace("Query", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            context.Variables.Set(Constants.QueryName, queryName);
        }
    }
}
