using System.Web;
using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Queries.SearchCustomers
{
    public class SearchCustomersQuery : IQuery, IRequest<SearchCustomersQueryResult>
    {
        public SearchCustomersOrderByEnum OrderBy { get; set; }
        public bool OrderByDescending { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }

        public override string ToString()
        {
            return ToQueryString();
        }

        public string ToQueryString()
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString.Add(nameof(OrderBy), OrderBy.ToString());
            queryString.Add(nameof(OrderByDescending), OrderByDescending ? "true" : "false");
            queryString.Add(nameof(PageIndex), PageIndex.ToString());
            queryString.Add(nameof(PageSize), PageSize.ToString());
            queryString.Add(nameof(SearchTerm), SearchTerm);

            return queryString.ToString();
        }
    }
}