using Demo.Application.Customers.Commands.CreateCustomer;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Controllers.Customers.Helpers
{
    internal class CustomerOperations
    {
        private HttpClient _httpClient;

        public CustomerOperations(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateCustomerCommand command)
        {
            return await _httpClient.PostAsJsonAsync("/api/customers", command);
        }

        public async Task<HttpResponseMessage> GetById(Guid id)
        {
            return await _httpClient.GetAsync($"/api/customers/{id}");
        }
    }
}
