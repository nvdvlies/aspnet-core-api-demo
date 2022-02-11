using Azure.Messaging.EventGrid;
using Demo.Application.Events.Commands.ProcessIncomingEvents;
using Demo.Infrastructure.Events;
using Demo.WebApi.Auth;
using Demo.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        [Authorize(nameof(Policies.User))] //TODO: [Authorize(nameof(Policies.AzureEventGrid))]
        public async Task<ActionResult> Post([FromBody] EventGridEvent[] eventGridEvents, CancellationToken cancellationToken)
        {
            var @events = eventGridEvents.Select(eventGridEvent => eventGridEvent.ToEvent());
            var command = new ProcessIncomingEventsCommand { Events = @events };

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
    }
}
