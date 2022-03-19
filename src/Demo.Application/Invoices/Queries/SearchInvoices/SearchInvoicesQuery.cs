using Demo.Application.Invoices.Queries.SearchInvoices.Dtos;
using MediatR;

namespace Demo.Application.Invoices.Queries.SearchInvoices
{
    public class SearchInvoicesQuery : IRequest<SearchInvoicesQueryResult>
    {
        public SearchInvoicesOrderByEnum OrderBy { get; set; }
        public bool OrderByDescending { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
    }
}