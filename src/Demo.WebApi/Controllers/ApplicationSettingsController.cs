using Demo.Application.ApplicationSettings.Queries.GetApplicationSettingsAuditlog;
using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings;
using Demo.Application.ApplicationSettings.Queries.GetApplicationSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class ApplicationSettingsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetApplicationSettingsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetApplicationSettingsQueryResult>> Get([FromQuery] GetApplicationSettingsQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Save(SaveApplicationSettingsCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("Auditlog")]
        [ProducesResponseType(typeof(GetApplicationSettingsAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetApplicationSettingsAuditlogQueryResult>> GetApplicationSettingsAuditlog([FromQuery] GetApplicationSettingsAuditlogQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}