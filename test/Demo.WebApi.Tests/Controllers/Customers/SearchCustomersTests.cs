using AutoFixture;
using Demo.Application.Customers.Queries.SearchCustomers;
using Demo.Application.Customers.Queries.SearchCustomers.Dtos;
using Demo.Domain.Customer;
using Demo.WebApi.Tests.Controllers.Customers.Helpers;
using Demo.WebApi.Tests.Helpers;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Demo.WebApi.Tests.Controllers.Customers
{
    [Collection(nameof(SharedFixture))]
    public class SearchCustomersTests : TestBase
    {
        public SearchCustomersTests(SharedFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task SearchCustomers_When_no_customers_exist_It_should_return_an_empty_list()
        {
            // Arrange
            await ResetDatabaseAsync();

            var query = new SearchCustomersQuery();

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().BeEmpty();

            content.PageIndex.Should().Be(query.PageIndex);
            content.PageSize.Should().Be(query.PageSize);
            content.TotalItems.Should().Be(0);
            content.TotalPages.Should().Be(0);
            content.HasNextPage.Should().Be(false);
            content.HasPreviousPage.Should().Be(false);
        }

        [Fact]
        public async Task SearchCustomers_When_1_customer_exists_It_should_return_1_customer()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var query = new SearchCustomersQuery();

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(1);

            content.PageIndex.Should().Be(query.PageIndex);
            content.PageSize.Should().Be(query.PageSize);
            content.TotalItems.Should().Be(1);
            content.TotalPages.Should().Be(1);
            content.HasNextPage.Should().Be(false);
            content.HasPreviousPage.Should().Be(false);

            content.Customers.First().Id.Should().Be(existingCustomer.Id);
            content.Customers.First().Name.Should().Be(existingCustomer.Name);
        }

        [Fact]
        public async Task SearchCustomers_When_2_customers_exist_It_should_return_2_customers()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer1 = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer1);

            var existingCustomer2 = new Customer { Id = Guid.NewGuid(), Name = "Test2" };
            await AddAsExistingEntityAsync(existingCustomer2);

            var query = new SearchCustomersQuery
            {
                OrderBy = SearchCustomersOrderByEnum.Name,
                OrderByDescending = false
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(2);

            content.PageIndex.Should().Be(query.PageIndex);
            content.PageSize.Should().Be(query.PageSize);
            content.TotalItems.Should().Be(2);
            content.TotalPages.Should().Be(1);
            content.HasNextPage.Should().Be(false);
            content.HasPreviousPage.Should().Be(false);

            content.Customers.First().Id.Should().Be(existingCustomer1.Id);
            content.Customers.First().Name.Should().Be(existingCustomer1.Name);

            content.Customers.Skip(1).First().Id.Should().Be(existingCustomer2.Id);
            content.Customers.Skip(1).First().Name.Should().Be(existingCustomer2.Name);
        }

        [Fact]
        public async Task SearchCustomers_When_18_customers_exist_and_pagesize_is_10_It_should_return_10_customers()
        {
            // Arrange
            await ResetDatabaseAsync();

            var numberOfExistingCustomers = 18;
            var existingCustomers = AutoFixture.CreateMany<Customer>(numberOfExistingCustomers);
            await AddAsExistingEntitiesAsync(existingCustomers);

            var query = new SearchCustomersQuery
            {
                PageSize = 10
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(query.PageSize);

            content.PageIndex.Should().Be(query.PageIndex);
            content.PageSize.Should().Be(query.PageSize);
            content.TotalItems.Should().Be(numberOfExistingCustomers);
            content.TotalPages.Should().Be(2);
            content.HasNextPage.Should().Be(true);
            content.HasPreviousPage.Should().Be(false);
        }

        [Fact]
        public async Task SearchCustomers_When_18_customers_exist_and_pagesize_is_10_and_page_index_is_1_It_should_return_8_customers()
        {
            // Arrange
            await ResetDatabaseAsync();

            var numberOfExistingCustomers = 18;

            var existingCustomers = AutoFixture.CreateMany<Customer>(numberOfExistingCustomers);
            await AddAsExistingEntitiesAsync(existingCustomers);

            var query = new SearchCustomersQuery
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(8);

            content.PageIndex.Should().Be(query.PageIndex);
            content.PageSize.Should().Be(query.PageSize);
            content.TotalItems.Should().Be(numberOfExistingCustomers);
            content.TotalPages.Should().Be(2);
            content.HasNextPage.Should().Be(false);
            content.HasPreviousPage.Should().Be(true);
        }

        [Fact]
        public async Task SearchCustomers_When_28_customers_exist_and_pagesize_is_10_and_page_index_is_1_It_should_return_10_customers()
        {
            // Arrange
            await ResetDatabaseAsync();

            var numberOfExistingCustomers = 28;
            var existingCustomers = AutoFixture.CreateMany<Customer>(numberOfExistingCustomers);
            await AddAsExistingEntitiesAsync(existingCustomers);

            var query = new SearchCustomersQuery
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(10);

            content.PageIndex.Should().Be(query.PageIndex);
            content.PageSize.Should().Be(query.PageSize);
            content.TotalItems.Should().Be(numberOfExistingCustomers);
            content.TotalPages.Should().Be(3);
            content.HasNextPage.Should().Be(true);
            content.HasPreviousPage.Should().Be(true);
        }

        [Fact]
        public async Task SearchCustomers_When_ordering_by_customer_code_ascending_It_should_return_list_in_correct_order()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer1 = new Customer { Id = Guid.NewGuid(), Code = 2, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer1);

            var existingCustomer2 = new Customer { Id = Guid.NewGuid(), Code = 1, Name = "Test2" };
            await AddAsExistingEntityAsync(existingCustomer2);

            var query = new SearchCustomersQuery
            {
                OrderBy = SearchCustomersOrderByEnum.Code,
                OrderByDescending = false
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(2);

            content.Customers.First().Id.Should().Be(existingCustomer2.Id);
            content.Customers.Skip(1).First().Id.Should().Be(existingCustomer1.Id);
        }

        [Fact]
        public async Task SearchCustomers_When_ordering_by_customer_code_descending_It_should_return_list_in_correct_order()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer1 = new Customer { Id = Guid.NewGuid(), Code = 2, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer1);

            var existingCustomer2 = new Customer { Id = Guid.NewGuid(), Code = 1, Name = "Test2" };
            await AddAsExistingEntityAsync(existingCustomer2);

            var query = new SearchCustomersQuery
            {
                OrderBy = SearchCustomersOrderByEnum.Code,
                OrderByDescending = true
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(2);

            content.Customers.First().Id.Should().Be(existingCustomer1.Id);
            content.Customers.Skip(1).First().Id.Should().Be(existingCustomer2.Id);
        }

        [Fact]
        public async Task SearchCustomers_When_ordering_by_customer_name_ascending_It_should_return_list_in_correct_order()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer1 = new Customer { Id = Guid.NewGuid(), Code = 2, Name = "B" };
            await AddAsExistingEntityAsync(existingCustomer1);

            var existingCustomer2 = new Customer { Id = Guid.NewGuid(), Code = 1, Name = "A" };
            await AddAsExistingEntityAsync(existingCustomer2);

            var query = new SearchCustomersQuery
            {
                OrderBy = SearchCustomersOrderByEnum.Name,
                OrderByDescending = false
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(2);

            content.Customers.First().Id.Should().Be(existingCustomer2.Id);
            content.Customers.Skip(1).First().Id.Should().Be(existingCustomer1.Id);
        }

        [Fact]
        public async Task SearchCustomers_When_ordering_by_customer_name_descending_It_should_return_list_in_correct_order()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer1 = new Customer { Id = Guid.NewGuid(), Code = 2, Name = "B" };
            await AddAsExistingEntityAsync(existingCustomer1);

            var existingCustomer2 = new Customer { Id = Guid.NewGuid(), Code = 1, Name = "A" };
            await AddAsExistingEntityAsync(existingCustomer2);

            var query = new SearchCustomersQuery
            {
                OrderBy = SearchCustomersOrderByEnum.Name,
                OrderByDescending = true
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(2);

            content.Customers.First().Id.Should().Be(existingCustomer1.Id);
            content.Customers.Skip(1).First().Id.Should().Be(existingCustomer2.Id);
        }

        [Fact]
        public async Task SearchCustomers_When_filter_on_name_It_should_exclude_non_matching_customers()
        {
            // Arrange
            await ResetDatabaseAsync();

            var existingCustomer1 = new Customer { Id = Guid.NewGuid(), Name = "Customer with Hello in name" };
            await AddAsExistingEntityAsync(existingCustomer1);

            var existingCustomer2 = new Customer { Id = Guid.NewGuid(), Name = "Customer without search term in name" };
            await AddAsExistingEntityAsync(existingCustomer2);

            var query = new SearchCustomersQuery
            {
                SearchTerm = "hello"
            };

            // Act
            var response = await Client.CustomersController().SearchAsync(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<SearchCustomersQueryResult>();
            content.Should().NotBeNull();
            content!.Customers.Should().NotBeEmpty();
            content.Customers.Should().HaveCount(1);

            content.Customers.First().Id.Should().Be(existingCustomer1.Id);
        }
    }
}
