using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors
{
    internal class CreateQueryResultOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var queryName = context.Variables.Get<string>(Constants.QueryName);

            changes.Add(new CreateNewClass(
                    directory: context.GetQueryDirectory(controllerName, queryName),
                    fileName: $"{queryName}QueryResult.cs",
                    content: GetTemplate(controllerName, queryName)
                ));

            return changes;
        }

        private static string GetTemplate(string controllerName, string queryName)
        {
            var code = @"
using Demo.Application.Shared.Dtos;

namespace Demo.Application.%CONTROLLERNAME%.Queries.%QUERYNAME%
{
    public class %QUERYNAME%QueryResult
    {
        public object X { get; set; }
    }
}
";
            code = code.Replace("%CONTROLLERNAME%", controllerName);
            code = code.Replace("%QUERYNAME%", queryName);
            return code;
        }
    }
}
