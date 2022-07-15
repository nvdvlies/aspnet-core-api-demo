using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Permissions.Queries.GetAllPermissionGroups;
using Demo.Application.Permissions.Queries.GetAllPermissions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class PermissionsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetAllPermissionsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetAllPermissionsQueryResult>> All([FromQuery] GetAllPermissionsQuery query,
            CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }


        [HttpGet("PermissionGroups")]
        [ProducesResponseType(typeof(GetAllPermissionGroupsQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetAllPermissionGroupsQueryResult>> GetAllPermissionGroups(
            [FromQuery] GetAllPermissionGroupsQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}
