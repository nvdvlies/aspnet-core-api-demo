using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQuery : IQuery, IRequest<GetInvoiceByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}