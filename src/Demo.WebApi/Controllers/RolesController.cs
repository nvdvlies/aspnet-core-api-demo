using Demo.Application.Roles.Commands.CreateRole;
using Demo.Application.Roles.Commands.DeleteRole;
using Demo.Application.Roles.Commands.UpdateRole;
using Demo.Application.Roles.Queries.GetRoleAuditlog;
using Demo.Application.Roles.Queries.GetRoleById;
using Demo.Application.Roles.Queries.RoleLookup;
using Demo.Application.Roles.Queries.SearchRoles;
using Demo.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class RolesController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(SearchRolesQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<SearchRolesQueryResult>> Search([FromQuery] SearchRolesQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }
        
        [HttpGet("{id:guid}", Name = nameof(GetRoleById))]
        [ProducesResponseType(typeof(GetRoleByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetRoleByIdQueryResult>> GetRoleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetRoleByIdQuery { Id = id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.Role == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        [HttpPost]
        [Authorize(nameof(Policies.Admin))]
        [ProducesResponseType(typeof(CreateRoleResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CreateRoleResponse>> Create(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(routeName: nameof(GetRoleById), routeValues: new { id = result.Id }, result);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(nameof(Policies.Admin))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Update([FromRoute] Guid id, UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            command.SetRoleId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize(nameof(Policies.Admin))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            command.SetRoleId(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
        
        [HttpGet("{id:guid}/Auditlog")]
        [Authorize(nameof(Policies.Admin))]
        [ProducesResponseType(typeof(GetRoleAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetRoleAuditlogQueryResult>> GetRoleAuditlog([FromRoute] Guid id, [FromQuery] GetRoleAuditlogQuery query, CancellationToken cancellationToken)
        {
            query.SetRoleId(id);

            return await Mediator.Send(query, cancellationToken);
        }

        [HttpGet("Lookup")]
        [ProducesResponseType(typeof(RoleLookupQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<RoleLookupQueryResult>> Lookup([FromQuery] RoleLookupQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}