using Demo.Application.Customers.Queries.CustomerLookup;
using Demo.Application.Customers.Commands.CreateCustomer;
using Demo.Application.Customers.Commands.DeleteCustomer;
using Demo.Application.Customers.Commands.UpdateCustomer;
using Demo.Application.Customers.Queries.GetCustomerAuditlog;
using Demo.Application.Customers.Queries.GetCustomerById;
using Demo.Application.Customers.Queries.SearchCustomers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(SearchCustomersQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<SearchCustomersQueryResult>> Search([FromQuery] SearchCustomersQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpGet("{id}", Name = nameof(GetCustomerById))]
        [ProducesResponseType(typeof(GetCustomerByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCustomerByIdQueryResult>> GetCustomerById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCustomerByIdQuery { Id = id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.Customer == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateCustomerResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CreateCustomerResponse>> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(routeName: nameof(GetCustomerById), routeValues: new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Update([FromRoute] Guid id, UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            command.SetCustomerId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            command.SetCustomerId(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpGet("{id}/Auditlog")]
        [ProducesResponseType(typeof(GetCustomerAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCustomerAuditlogQueryResult>> GetCustomerAuditlog([FromRoute] Guid id, [FromQuery] GetCustomerAuditlogQuery query, CancellationToken cancellationToken)
        {
            query.SetCustomerId(id);

            return await Mediator.Send(query, cancellationToken);
        }


        [HttpGet("Lookup")]
        [ProducesResponseType(typeof(CustomerLookupQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CustomerLookupQueryResult>> Lookup([FromQuery] CustomerLookupQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}