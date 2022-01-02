using Azure.Messaging.EventGrid;
using Demo.Application.Events.Commands.ProcessIncomingEvents;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        //[Authorize(nameof(Policies.AzureEventGrid))]
        public async Task<ActionResult> Post([FromBody] EventGridEvent[] eventGridEvents, CancellationToken cancellationToken)
        {
            var command = new ProcessIncomingEventsCommand { EventGridEvents = eventGridEvents };

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
    }
}
