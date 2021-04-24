using MediatR;
using System;

namespace Demo.Application.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQuery : IRequest<GetInvoiceByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}