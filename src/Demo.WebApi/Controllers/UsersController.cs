using Demo.Application.Users.Commands.CreateUser;
using Demo.Application.Users.Commands.DeleteUser;
using Demo.Application.Users.Commands.UpdateUser;
using Demo.Application.Users.Queries.GetUserAuditlog;
using Demo.Application.Users.Queries.GetUserById;
using Demo.Application.Users.Queries.SearchUsers;
using Demo.Application.Users.Queries.UserLookup;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(SearchUsersQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<SearchUsersQueryResult>> Search([FromQuery] SearchUsersQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpGet("{id}", Name = nameof(GetUserById))]
        [ProducesResponseType(typeof(GetUserByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetUserByIdQueryResult>> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.User == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(routeName: nameof(GetUserById), routeValues: new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Update([FromRoute] Guid id, UpdateUserCommand command, CancellationToken cancellationToken)
        {
            command.SetUserId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, DeleteUserCommand command, CancellationToken cancellationToken)
        {
            command.SetUserId(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }


        [HttpGet("{id}/Auditlog")]
        [ProducesResponseType(typeof(GetUserAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetUserAuditlogQueryResult>> GetUserAuditlog([FromRoute] Guid id, [FromQuery] GetUserAuditlogQuery query, CancellationToken cancellationToken)
        {
            query.SetUserId(id);

            return await Mediator.Send(query, cancellationToken);
        }


        [HttpGet("Lookup")]
        [ProducesResponseType(typeof(UserLookupQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserLookupQueryResult>> UserLookup([FromQuery] UserLookupQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}