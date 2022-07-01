using System;
using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors
{
    internal class AddEndpointToControllerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var queryEndpointType = context.Variables.Get<QueryEndpointTypes>(Constants.QueryEndpointType);
            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var queryName = context.Variables.Get<string>(Constants.QueryName);

            var entityName = controllerName.EndsWith("s") ? controllerName[..^1] : controllerName;
            var endpointName = queryName.Replace(entityName, string.Empty, StringComparison.CurrentCultureIgnoreCase);
            endpointName = string.Equals(endpointName, "searchs", StringComparison.OrdinalIgnoreCase)
                ? endpointName[..^1]
                : endpointName;

            changes.Add(new UpdateExistingClassAtMarker(
                context.GetControllersDirectory(),
                context.GetControllerFileName(controllerName),
                Tool.Constants.ScaffoldMarkerEndpoint,
                GetTemplate(queryEndpointType, queryName, entityName, endpointName)
            ));

            return changes;
        }

        private static string GetTemplate(QueryEndpointTypes queryEndpointType, string queryName, string entityName,
            string endpointName)
        {
            switch (queryEndpointType)
            {
                case QueryEndpointTypes.Search:
                    return GetSearchTemplate(queryName, endpointName);
                case QueryEndpointTypes.GetById:
                    return GetGetByIdTemplate(queryName);
                case QueryEndpointTypes.SubEndpoint:
                    return GetSubEndpointTemplate(queryName, entityName);
                default:
                    throw new Exception($"QueryEndpointType '{queryEndpointType}' not supported.");
            }
        }

        private static string GetSearchTemplate(string queryName, string endpointName)
        {
            var code = @"
        [HttpGet]
        [ProducesResponseType(typeof(%QUERYNAME%QueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<%QUERYNAME%QueryResult>> %ENDPOINTNAME%([FromQuery] %QUERYNAME%Query query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }
";
            code = code.Replace("%QUERYNAME%", queryName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }

        private static string GetGetByIdTemplate(string queryName)
        {
            var code = @"
        [HttpGet(""{id:guid}"", Name = nameof(%QUERYNAME%))]
        [ProducesResponseType(typeof(%QUERYNAME%QueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<%QUERYNAME%QueryResult>> %QUERYNAME%([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new %QUERYNAME%Query { Id = id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.X == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
";
            code = code.Replace("%QUERYNAME%", queryName);
            return code;
        }

        private static string GetSubEndpointTemplate(string queryName, string entityName)
        {
            var code = @"
        [HttpGet(""{id:guid}/%ROUTE%"")]
        [ProducesResponseType(typeof(%QUERYNAME%QueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<%QUERYNAME%QueryResult>> %QUERYNAME%([FromRoute] Guid id, [FromQuery] %QUERYNAME%Query query, CancellationToken cancellationToken)
        {
            query.Set%ENTITY%Id(id);

            return await Mediator.Send(query, cancellationToken);
        }
";

            var route = queryName.Replace($"Get{entityName}", string.Empty);

            code = code.Replace("%ROUTE%", route);
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%QUERYNAME%", queryName);
            return code;
        }
    }
}