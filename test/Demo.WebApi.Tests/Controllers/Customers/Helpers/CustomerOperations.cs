using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Application.Customers.Commands.CreateCustomer;
using Demo.Application.Customers.Commands.UpdateCustomer;
using Demo.Application.Customers.Queries.SearchCustomers;

namespace Demo.WebApi.Tests.Controllers.Customers.Helpers
{
    internal class CustomerOperations
    {
        private readonly HttpClient _httpClient;

        public CustomerOperations(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> SearchAsync(SearchCustomersQuery query)
        {
            return _httpClient.GetAsync($"/api/customers?{query.ToQueryString()}");
        }

        public Task<HttpResponseMessage> GetById(Guid id)
        {
            return _httpClient.GetAsync($"/api/customers/{id}");
        }

        public Task<HttpResponseMessage> CreateAsync(CreateCustomerCommand command)
        {
            return _httpClient.PostAsJsonAsync("/api/customers", command);
        }

        public Task<HttpResponseMessage> UpdateAsync(UpdateCustomerCommand command)
        {
            return _httpClient.PutAsJsonAsync($"/api/customers/{command.Id}", command);
        }
    }
}