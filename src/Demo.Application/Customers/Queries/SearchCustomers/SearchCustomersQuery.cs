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
    }
}