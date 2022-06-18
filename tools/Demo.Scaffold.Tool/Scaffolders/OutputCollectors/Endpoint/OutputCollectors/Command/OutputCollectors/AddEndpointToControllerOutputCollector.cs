using System;
using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors
{
    internal class AddEndpointToControllerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var commandEndpointType = context.Variables.Get<CommandEndpointTypes>(Constants.CommandEndpointType);
            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);

            var entityName = controllerName.EndsWith("s") ? controllerName[..^1] : controllerName;
            var endpointName = commandName.Replace(entityName, string.Empty, StringComparison.CurrentCultureIgnoreCase);

            changes.Add(new UpdateExistingClassAtMarker(
                context.GetControllersDirectory(),
                context.GetControllerFileName(controllerName),
                Tool.Constants.ScaffoldMarkerEndpoint,
                GetTemplate(commandEndpointType, entityName, commandName, endpointName)
            ));

            return changes;
        }

        private static string GetTemplate(CommandEndpointTypes commandEndpointType, string entityName,
            string commandName, string endpointName)
        {
            switch (commandEndpointType)
            {
                case CommandEndpointTypes.Create:
                    return GetPostTemplate(entityName, commandName, endpointName);
                case CommandEndpointTypes.Update:
                    return GetPutTemplate(entityName, commandName, endpointName);
                case CommandEndpointTypes.Delete:
                    return GetDeleteTemplate(entityName, commandName, endpointName);
                case CommandEndpointTypes.CreateSubEndpoint:
                    return GetPostSubEndpointTemplate(entityName, commandName, endpointName);
                case CommandEndpointTypes.UpdateSubEndpoint:
                    return GetUpdateSubEndpointTemplate(entityName, commandName, endpointName);
                case CommandEndpointTypes.DeleteSubEndpoint:
                    return GetDeleteSubEndpointTemplate(entityName, commandName, endpointName);
                default:
                    throw new Exception($"CommandEndpointType '{commandEndpointType}' not supported.");
            }
        }

        private static string GetPostTemplate(string entityName, string commandName, string endpointName)
        {
            var code = @"
        [HttpPost]
        [ProducesResponseType(typeof(%COMMANDNAME%Response), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<%COMMANDNAME%Response>> %ENDPOINTNAME%(%COMMANDNAME%Command command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(routeName: nameof(Get%ENTITY%ById), routeValues: new { id = result.Id }, result);
        }
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }

        private static string GetPutTemplate(string entityName, string commandName, string endpointName)
        {
            var code = @"
        [HttpPut(""{id:guid}"")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> %ENDPOINTNAME%([FromRoute] Guid id, %COMMANDNAME%Command command, CancellationToken cancellationToken)
        {
            command.Set%ENTITY%Id(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }

        private static string GetDeleteTemplate(string entityName, string commandName, string endpointName)
        {
            var code = @"
        [HttpDelete(""{id:guid}"")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> %ENDPOINTNAME%([FromRoute] Guid id, %COMMANDNAME%Command command, CancellationToken cancellationToken)
        {
            command.Set%ENTITY%Id(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }

        private static string GetPostSubEndpointTemplate(string entityName, string commandName, string endpointName)
        {
            var code = @"
        [HttpPost(""{id:guid}/%ENDPOINTNAME%"")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> %ENDPOINTNAME%([FromRoute] Guid id, %COMMANDNAME%Command command, CancellationToken cancellationToken)
        {
            command.Set%ENTITY%Id(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }

        private static string GetUpdateSubEndpointTemplate(string entityName, string commandName, string endpointName)
        {
            var code = @"
        [HttpPut(""{id:guid}/%ENDPOINTNAME%"")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> %ENDPOINTNAME%([FromRoute] Guid id, %COMMANDNAME%Command command, CancellationToken cancellationToken)
        {
            command.Set%ENTITY%Id(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }

        private static string GetDeleteSubEndpointTemplate(string entityName, string commandName, string endpointName)
        {
            var code = @"
        [HttpDelete(""{id:guid}/%ENDPOINTNAME%"")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> %ENDPOINTNAME%([FromRoute] Guid id, %COMMANDNAME%Command command, CancellationToken cancellationToken)
        {
            command.Set%ENTITY%Id(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENDPOINTNAME%", endpointName);
            return code;
        }
    }
}
