using MediatR;
using System;

namespace Demo.Application.Invoices.Queries.GetInvoiceAuditlog
{
    public class GetInvoiceAuditlogQuery : IRequest<GetInvoiceAuditlogQueryResult>
    {
        internal Guid InvoiceId { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public void SetInvoiceId(Guid id)
        {
            InvoiceId = id;
        }
    }
}