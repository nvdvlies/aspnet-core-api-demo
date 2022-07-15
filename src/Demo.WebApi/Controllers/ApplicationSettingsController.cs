using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings;
using Demo.Application.ApplicationSettings.Queries.GetApplicationSettings;
using Demo.Application.ApplicationSettings.Queries.GetApplicationSettingsAuditlog;
using Demo.WebApi.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class ApplicationSettingsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetApplicationSettingsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetApplicationSettingsQueryResult>> Get(
            [FromQuery] GetApplicationSettingsQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpPut]
        [Permission(Domain.Role.Permissions.ApplicationSettingsWrite)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Save(SaveApplicationSettingsCommand command,
            CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("Auditlog")]
        [Permission(Domain.Role.Permissions.ApplicationSettingsRead)]
        [ProducesResponseType(typeof(GetApplicationSettingsAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetApplicationSettingsAuditlogQueryResult>> GetApplicationSettingsAuditlog(
            [FromQuery] GetApplicationSettingsAuditlogQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}
