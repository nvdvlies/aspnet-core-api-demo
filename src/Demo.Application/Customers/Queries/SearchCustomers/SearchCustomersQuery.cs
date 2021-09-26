using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using MediatR;

namespace Demo.Application.Customers.Queries.SearchCustomers
{
    public class SearchCustomersQuery : IRequest<SearchCustomersQueryResult>
    {
        public SearchCustomersOrderByEnum OrderBy { get; set; }
        public bool OrderByDescending { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public override string ToString()
        {
            return ToQueryString();
        }

        public string ToQueryString()
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add(nameof(OrderBy), OrderBy.ToString());
            queryString.Add(nameof(OrderByDescending), OrderByDescending ? "true" : "false");
            queryString.Add(nameof(PageIndex), PageIndex.ToString());
            queryString.Add(nameof(PageSize), PageSize.ToString());
            queryString.Add(nameof(Name), Name);

            return queryString.ToString();
        }
    }
}