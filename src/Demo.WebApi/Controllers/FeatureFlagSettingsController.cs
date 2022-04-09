using Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class FeatureFlagSettingsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetFeatureFlagSettingsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetFeatureFlagSettingsQueryResult>> Get([FromQuery] GetFeatureFlagSettingsQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }


        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Save(SaveFeatureFlagSettingsCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}