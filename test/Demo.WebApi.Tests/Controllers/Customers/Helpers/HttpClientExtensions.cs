using System.Net.Http;

namespace Demo.WebApi.Tests.Controllers.Customers.Helpers
{
    internal static class HttpClientExtensions
    {
        public static CustomerOperations CustomersController(this HttpClient httpClient)
        {
            return new CustomerOperations(httpClient);
        }
    }
}
