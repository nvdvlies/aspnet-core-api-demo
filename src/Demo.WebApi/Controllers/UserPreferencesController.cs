using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.UserPreferences.Commands.SaveUserPreferences;
using Demo.Application.UserPreferences.Queries.GetUserPreferences;
using Demo.Application.UserPreferences.Queries.GetUserPreferencesAuditlog;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class UserPreferencesController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetUserPreferencesQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetUserPreferencesQueryResult>> Get([FromQuery] GetUserPreferencesQuery query,
            CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Save(SaveUserPreferencesCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("Auditlog")]
        [ProducesResponseType(typeof(GetUserPreferencesAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetUserPreferencesAuditlogQueryResult>> GetUserPreferencesAuditlog(
            [FromQuery] GetUserPreferencesAuditlogQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}
