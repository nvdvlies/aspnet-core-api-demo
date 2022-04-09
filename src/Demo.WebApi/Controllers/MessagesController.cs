using Azure.Messaging.ServiceBus;
using Demo.Infrastructure.Messages;
using Demo.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ApiControllerBase
    {
        [Authorize(nameof(Policies.Machine))]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [Authorize(nameof(Policies.Machine))]
        public async Task<ActionResult> Post([FromBody] ServiceBusMessage serviceBusMessage, CancellationToken cancellationToken)
        {
            var message = serviceBusMessage.ToMessage();

            await Mediator.Send(message, cancellationToken);

            return Ok();
        }
    }
}
