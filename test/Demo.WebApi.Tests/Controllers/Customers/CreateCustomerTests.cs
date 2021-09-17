using Demo.Application.Customers.Commands.CreateCustomer;
using Demo.Application.Customers.Events;
using Demo.Domain.Customer.BusinessComponent.Interfaces;
using Demo.WebApi.Tests.Controllers.Customers.Helpers;
using Demo.WebApi.Tests.Helpers;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Demo.WebApi.Tests.Controllers.Customers
{
    [Collection(nameof(SharedFixture))]
    public class CreateCustomerTests : TestBase
    {
        public CreateCustomerTests(SharedFixture fixture) : base(fixture) 
        { 
        }

        [Fact]
        public async Task CreateCustomer_When_a_customer_can_be_created_It_should_return_statusCode_Created()
        {
            // Arrange
            var command = new CreateCustomerCommand { 
                Name = "Test"
            };

            // Act
            var response = await _client.CustomersController().CreateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();
            content.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateCustomer_When_a_customer_is_created_It_should_publish_a_CustomerCreated_event()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "Test"
            };

            Guid idFromEvent = Guid.Empty;
            using var subscription = _hubConnection.On(nameof(ICustomerEventHub.CustomerCreated), (Guid id, Guid createdBy) =>
            {
                idFromEvent = id;
            });

            // Act
            var response = await _client.CustomersController().CreateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();
            content.Id.Should().NotBeEmpty();
            WithRetry(() => idFromEvent.Should().Be(content.Id), 1.Seconds(), 10.Milliseconds());
        }

        [Fact]
        public async Task CreateCustomer_When_a_command_validator_throws_a_ValidationException_It_should_return_statusCode_BadRequest()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = string.Empty 
            };

            // Act
            var response = await _client.CustomersController().CreateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Detail.Should().Contain("'Name' must not be empty");
        }

        [Fact]
        public async Task CreateCustomer_When_a_businesscomponent_validator_throws_a_DomainValidationException_It_should_return_statusCode_BadRequest()
        {
            await Task.CompletedTask; // TODO
        }

        [Fact]
        public async Task CreateCustomer_When_a_businesscomponent_hook_throws_a_DomainException_It_should_return_statusCode_BadRequest()
        {
            await Task.CompletedTask; // TODO
        }

        [Fact]
        public async Task CreateCustomer_When_an_unhandled_exception_occurs_It_should_return_statusCode_InternalServerError()
        {
            // Arrange
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var mock = new Mock<ICustomerBusinessComponent>();
                        mock.Setup(x => x.CreateAsync(It.IsAny<CancellationToken>())).Throws(new Exception("TestException"));

                        services.AddSingleton(mock.Object);
                    });
                })
                .CreateClient();

            var command = new CreateCustomerCommand
            {
                Name = "Test"
            };

            // Act
            var response = await client.CustomersController().CreateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Title.Should().Be("TestException");
        }
    }
}
