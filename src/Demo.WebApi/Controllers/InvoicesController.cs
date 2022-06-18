using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Invoices.Commands.CopyInvoice;
using Demo.Application.Invoices.Commands.CreateInvoice;
using Demo.Application.Invoices.Commands.CreditInvoice;
using Demo.Application.Invoices.Commands.DeleteInvoice;
using Demo.Application.Invoices.Commands.MarkInvoiceAsCancelled;
using Demo.Application.Invoices.Commands.MarkInvoiceAsPaid;
using Demo.Application.Invoices.Commands.MarkInvoiceAsSent;
using Demo.Application.Invoices.Commands.UpdateInvoice;
using Demo.Application.Invoices.Queries.GetInvoiceAuditlog;
using Demo.Application.Invoices.Queries.GetInvoiceById;
using Demo.Application.Invoices.Queries.InvoiceLookup;
using Demo.Application.Invoices.Queries.SearchInvoices;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    public class InvoicesController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(SearchInvoicesQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<SearchInvoicesQueryResult>> Search([FromQuery] SearchInvoicesQuery query,
            CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpGet("{id:guid}", Name = nameof(GetInvoiceById))]
        [ProducesResponseType(typeof(GetInvoiceByIdQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetInvoiceByIdQueryResult>> GetInvoiceById([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetInvoiceByIdQuery { Id = id };
            var result = await Mediator.Send(query, cancellationToken);

            if (result?.Invoice == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateInvoiceResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CreateInvoiceResponse>> Create(CreateInvoiceCommand command,
            CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(nameof(GetInvoiceById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Update([FromRoute] Guid id, UpdateInvoiceCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, DeleteInvoiceCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPut("{id:guid}/MarkAsSent")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> MarkAsSent([FromRoute] Guid id, MarkInvoiceAsSentCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:guid}/MarkAsPaid")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> MarkAsPaid([FromRoute] Guid id, MarkInvoiceAsPaidCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:guid}/MarkAsCancelled")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> MarkAsCancelled([FromRoute] Guid id, MarkInvoiceAsCancelledCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("{id:guid}/Auditlog")]
        [ProducesResponseType(typeof(GetInvoiceAuditlogQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GetInvoiceAuditlogQueryResult>> GetInvoiceAuditlog([FromRoute] Guid id,
            [FromQuery] GetInvoiceAuditlogQuery query, CancellationToken cancellationToken)
        {
            query.SetInvoiceId(id);

            return await Mediator.Send(query, cancellationToken);
        }

        [HttpPost("{id:guid}/Copy")]
        [ProducesResponseType(typeof(CopyInvoiceResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Copy([FromRoute] Guid id, CopyInvoiceCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(nameof(GetInvoiceById), new { id = result.Id }, result);
        }

        [HttpPost("{id:guid}/Credit")]
        [ProducesResponseType(typeof(CreditInvoiceResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Credit([FromRoute] Guid id, CreditInvoiceCommand command,
            CancellationToken cancellationToken)
        {
            command.SetInvoiceId(id);

            var result = await Mediator.Send(command, cancellationToken);

            return CreatedAtRoute(nameof(GetInvoiceById), new { id = result.Id }, result);
        }

        [HttpGet("Lookup")]
        [ProducesResponseType(typeof(InvoiceLookupQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<InvoiceLookupQueryResult>> Lookup([FromQuery] InvoiceLookupQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        // SCAFFOLD-MARKER: ENDPOINT
    }
}
