using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors
{
    internal class CreateQueryHandlerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var queryName = context.Variables.Get<string>(Constants.QueryName);

            changes.Add(new CreateNewClass(
                context.GetQueryDirectory(controllerName, queryName),
                $"{queryName}QueryHandler.cs",
                GetTemplate(controllerName, queryName)
            ));

            return changes;
        }

        private static string GetTemplate(string controllerName, string queryName)
        {
            var code = @"
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.%CONTROLLERNAME%.Queries.%QUERYNAME%
{
    public class %QUERYNAME%QueryHandler : IRequestHandler<%QUERYNAME%Query, %QUERYNAME%QueryResult>
    {
        public %QUERYNAME%QueryHandler()
        {
        }

        public async Task<%QUERYNAME%QueryResult> Handle(%QUERYNAME%Query request, CancellationToken cancellationToken)
        {
            return new %QUERYNAME%QueryResult();
        }
    }
}
";
            code = code.Replace("%CONTROLLERNAME%", controllerName);
            code = code.Replace("%QUERYNAME%", queryName);
            return code;
        }
    }
}