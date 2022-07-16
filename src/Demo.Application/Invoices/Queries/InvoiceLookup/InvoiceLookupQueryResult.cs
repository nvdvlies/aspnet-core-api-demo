using System.Collections.Generic;
using Demo.Application.Invoices.Queries.InvoiceLookup.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Invoices.Queries.InvoiceLookup;

public class InvoiceLookupQueryResult : BasePaginatedResult
{
    public IEnumerable<InvoiceLookupDto> Invoices { get; set; }
}
