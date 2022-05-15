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
    public class MessagesController : ApiControllerBase
    {
        [HttpPost]
        [Authorize(nameof(Policies.Machine))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Post([FromBody] ServiceBusMessage serviceBusMessage, CancellationToken cancellationToken)
        {
            var message = serviceBusMessage.ToMessage();

            await Mediator.Send(message, cancellationToken);

            return Ok();
        }
    }
}
