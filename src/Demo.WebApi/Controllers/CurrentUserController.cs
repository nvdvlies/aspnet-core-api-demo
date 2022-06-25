using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.CurrentUser.Queries.GetCurrentUserFeatureFlags;
using Demo.Application.Users.Queries.GetUserById;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class CurrentUserController : ApiControllerBase
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public CurrentUserController(
            ICurrentUserIdProvider currentUserIdProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUserByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetUserByIdQueryResult>> GetCurrentUserDetails(
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery { Id = _currentUserIdProvider.Id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.User == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("FeatureFlags")]
        [ProducesResponseType(typeof(GetCurrentUserFeatureFlagsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCurrentUserFeatureFlagsQueryResult>> GetCurrentUserFeatureFlags(
            [FromQuery] GetCurrentUserFeatureFlagsQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}
