using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings;
using Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettingsAuditlog;
using Demo.WebApi.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers;

public class FeatureFlagSettingsController : ApiControllerBase
{
    [HttpGet]
    [Permission(Domain.Role.Permissions.FeatureFlagSettingsRead)]
    [ProducesResponseType(typeof(GetFeatureFlagSettingsQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<GetFeatureFlagSettingsQueryResult>> Get(
        [FromQuery] GetFeatureFlagSettingsQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpPut]
    [Permission(Domain.Role.Permissions.FeatureFlagSettingsWrite)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Save(SaveFeatureFlagSettingsCommand command,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("Auditlog")]
    [Permission(Domain.Role.Permissions.FeatureFlagSettingsRead)]
    [ProducesResponseType(typeof(GetFeatureFlagSettingsAuditlogQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<GetFeatureFlagSettingsAuditlogQueryResult>> GetFeatureFlagSettingsAuditlog(
        [FromQuery] GetFeatureFlagSettingsAuditlogQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    // SCAFFOLD-MARKER: ENDPOINT
}
