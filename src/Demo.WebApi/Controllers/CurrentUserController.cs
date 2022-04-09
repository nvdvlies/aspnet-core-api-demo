using Demo.Application.Users.Queries.GetUserById;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class CurrentUserController : ApiControllerBase
    {
        private readonly ICurrentUser _currentUser;

        public CurrentUserController(
            ICurrentUser currentUser
        )
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUserByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetUserByIdQueryResult>> GetCurrentUserDetails( CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery { Id = _currentUser.Id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.User == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}