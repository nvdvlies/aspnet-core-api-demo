using Azure.Messaging.EventGrid;
using Demo.Infrastructure.Events;
using Demo.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class EventsController : ApiControllerBase
    {
        [HttpPost]
        [Authorize(nameof(Policies.Machine))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Post([FromBody] EventGridEvent[] eventGridEvents, CancellationToken cancellationToken)
        {
            var @events = eventGridEvents.Select(eventGridEvent => eventGridEvent.ToEvent());

            foreach (var @event in @events)
            {
                await Mediator.Publish(@event, cancellationToken);
            }

            return Ok();
        }
    }
}
