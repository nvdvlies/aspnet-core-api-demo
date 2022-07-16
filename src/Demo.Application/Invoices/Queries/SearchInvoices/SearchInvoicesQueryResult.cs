using System.Collections.Generic;
using Demo.Application.Invoices.Queries.SearchInvoices.Dtos;
using Demo.Application.Shared.Models;

namespace Demo.Application.Invoices.Queries.SearchInvoices;

public class SearchInvoicesQueryResult : BasePaginatedResult
{
    public IEnumerable<SearchInvoiceDto> Invoices { get; set; }
}
