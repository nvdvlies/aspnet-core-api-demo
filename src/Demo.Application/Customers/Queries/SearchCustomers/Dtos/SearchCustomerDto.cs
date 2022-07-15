using System;

namespace Demo.Application.Customers.Queries.SearchCustomers.Dtos
{
    public class SearchCustomerDto
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
