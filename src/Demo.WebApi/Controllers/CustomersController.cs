using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Customers.Commands.CreateCustomer;
using Demo.Application.Customers.Commands.DeleteCustomer;
using Demo.Application.Customers.Commands.UpdateCustomer;
using Demo.Application.Customers.Queries.CustomerLookup;
using Demo.Application.Customers.Queries.GetCustomerAuditlog;
using Demo.Application.Customers.Queries.GetCustomerById;
using Demo.Application.Customers.Queries.SearchCustomers;
using Demo.WebApi.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        [HttpGet]
        [Permission(Domain.Role.Permissions.CustomersRead)]
        [ProducesResponseType(typeof(SearchCustomersQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<SearchCustomersQueryResult>> Search([FromQuery] SearchCustomersQuery query,
            CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpGet("{id:guid}", Name = nameof(GetCustomerById))]
        [Permission(Domain.Role.Permissions.CustomersRead)]
        [ProducesResponseType(typeof(GetCustomerByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCustomerByIdQueryResult>> GetCustomerById([FromRoute] Guid id,
            CancellationToken cancellationToken)
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
        [Permission(Domain.Role.Permissions.CustomersWrite)]
        [ProducesResponseType(typeof(CreateCustomerResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CreateCustomerResponse>> Create(CreateCustomerCommand command,
            CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(nameof(GetCustomerById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Permission(Domain.Role.Permissions.CustomersWrite)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Update([FromRoute] Guid id, UpdateCustomerCommand command,
            CancellationToken cancellationToken)
        {
            command.SetCustomerId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Permission(Domain.Role.Permissions.CustomersWrite)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, DeleteCustomerCommand command,
            CancellationToken cancellationToken)
        {
            command.SetCustomerId(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpGet("{id:guid}/Auditlog")]
        [Permission(Domain.Role.Permissions.CustomersRead)]
        [ProducesResponseType(typeof(GetCustomerAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetCustomerAuditlogQueryResult>> GetCustomerAuditlog([FromRoute] Guid id,
            [FromQuery] GetCustomerAuditlogQuery query, CancellationToken cancellationToken)
        {
            query.SetCustomerId(id);

            return await Mediator.Send(query, cancellationToken);
        }


        [HttpGet("Lookup")]
        [ProducesResponseType(typeof(CustomerLookupQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CustomerLookupQueryResult>> Lookup([FromQuery] CustomerLookupQuery query,
            CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}
