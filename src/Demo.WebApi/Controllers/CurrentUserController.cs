using Demo.Application.CurrentUser.Commands.ChangePassword;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.CurrentUser.Commands.UpdateCurrentUser;
using Demo.Application.CurrentUser.Queries.GetCurrentUser;
using Demo.Application.CurrentUser.Queries.GetCurrentUserFeatureFlags;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class CurrentUserController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetCurrentUserQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCurrentUserQueryResult>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var query = new GetCurrentUserQuery();
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.CurrentUser == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Update(UpdateCurrentUserCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("FeatureFlags")]
        [ProducesResponseType(typeof(GetCurrentUserFeatureFlagsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCurrentUserFeatureFlagsQueryResult>> GetCurrentUserFeatureFlags(
            [FromQuery] GetCurrentUserFeatureFlagsQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }


        [HttpPost("ChangePassword")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}