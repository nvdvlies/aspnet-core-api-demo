using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors;

internal class CreateQueryOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var queryEndpointType = context.Variables.Get<QueryEndpointTypes>(Constants.QueryEndpointType);
        var controllerName = context.Variables.Get<string>(Constants.ControllerName);
        var queryName = context.Variables.Get<string>(Constants.QueryName);

        var entityName = controllerName.EndsWith("s") ? controllerName[..^1] : controllerName;

        changes.Add(new CreateNewClass(
            context.GetQueryDirectory(controllerName, queryName),
            $"{queryName}Query.cs",
            GetTemplate(queryEndpointType, controllerName, queryName, entityName)
        ));

        return changes;
    }

    private static string GetTemplate(QueryEndpointTypes queryEndpointType, string controllerName, string queryName,
        string entityName)
    {
        var code = queryEndpointType == QueryEndpointTypes.SubEndpoint
            ? @"
using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.%CONTROLLERNAME%.Queries.%QUERYNAME%
{
    public class %QUERYNAME%Query : IQuery, IRequest<%QUERYNAME%QueryResult>
    {
        internal Guid Id { get; set; }

        public void Set%ENTITYNAME%Id(Guid id)
        {
            Id = id;
        }
    }
}
"
            : @"
using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.%CONTROLLERNAME%.Queries.%QUERYNAME%
{
    public class %QUERYNAME%Query : IQuery, IRequest<%QUERYNAME%QueryResult>
    {
        public Guid Id { get; set; }
    }
}
";
        code = code.Replace("%CONTROLLERNAME%", controllerName);
        code = code.Replace("%QUERYNAME%", queryName);
        code = code.Replace("%ENTITYNAME%", entityName);
        return code;
    }
}
