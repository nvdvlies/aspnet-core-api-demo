using Demo.Application.Locations.Queries.LocationLookup;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers;

public class LocationsController : ApiControllerBase
{
    [HttpGet("Lookup")]
    [ProducesResponseType(typeof(LocationLookupQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<LocationLookupQueryResult>> Lookup([FromQuery] LocationLookupQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    // SCAFFOLD-MARKER: ENDPOINT
}
