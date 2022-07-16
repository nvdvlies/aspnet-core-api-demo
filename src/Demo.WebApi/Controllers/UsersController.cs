using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Users.Commands.CreateUser;
using Demo.Application.Users.Commands.DeleteUser;
using Demo.Application.Users.Commands.ResetPassword;
using Demo.Application.Users.Commands.UpdateUser;
using Demo.Application.Users.Queries.GetUserAuditlog;
using Demo.Application.Users.Queries.GetUserById;
using Demo.Application.Users.Queries.IsEmailAvailable;
using Demo.Application.Users.Queries.SearchUsers;
using Demo.Application.Users.Queries.UserLookup;
using Demo.WebApi.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet]
    [Permission(Domain.Role.Permissions.UsersRead)]
    [ProducesResponseType(typeof(SearchUsersQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<SearchUsersQueryResult>> Search([FromQuery] SearchUsersQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserById))]
    [Permission(Domain.Role.Permissions.UsersRead)]
    [ProducesResponseType(typeof(GetUserByIdQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<GetUserByIdQueryResult>> GetUserById([FromRoute] Guid id,
        CancellationToken cancellationToken)
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
    [Permission(Domain.Role.Permissions.UsersWrite)]
    [ProducesResponseType(typeof(CreateUserResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<CreateUserResponse>> Create(CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        return CreatedAtRoute(nameof(GetUserById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Permission(Domain.Role.Permissions.UsersWrite)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Update([FromRoute] Guid id, UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        command.SetUserId(id);

        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Permission(Domain.Role.Permissions.UsersWrite)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        command.SetUserId(id);

        await Mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpGet("{id:guid}/Auditlog")]
    [Permission(Domain.Role.Permissions.UsersWrite)]
    [ProducesResponseType(typeof(GetUserAuditlogQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<GetUserAuditlogQueryResult>> GetUserAuditlog([FromRoute] Guid id,
        [FromQuery] GetUserAuditlogQuery query, CancellationToken cancellationToken)
    {
        query.SetUserId(id);

        return await Mediator.Send(query, cancellationToken);
    }


    [HttpGet("Lookup")]
    [ProducesResponseType(typeof(UserLookupQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<UserLookupQueryResult>> Lookup([FromQuery] UserLookupQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("IsEmailAvailable")]
    [ProducesResponseType(typeof(IsEmailAvailableQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<IsEmailAvailableQueryResult>> IsEmailAvailable(
        [FromQuery] IsEmailAvailableQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpPost("{id:guid}/ResetPassword")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> ResetPassword([FromRoute] Guid id, ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        command.SetUserId(id);

        await Mediator.Send(command, cancellationToken);

        return Ok();
    }

    // SCAFFOLD-MARKER: ENDPOINT
}
