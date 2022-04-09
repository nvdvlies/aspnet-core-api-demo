using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettingsAuditlog;
using Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(nameof(Policies.Admin))]
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Save(SaveFeatureFlagSettingsCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("{id}/Auditlog")]
        [ProducesResponseType(typeof(GetFeatureFlagSettingsAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetFeatureFlagSettingsAuditlogQueryResult>> GetFeatureFlagSettingsAuditlog([FromQuery] GetFeatureFlagSettingsAuditlogQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}