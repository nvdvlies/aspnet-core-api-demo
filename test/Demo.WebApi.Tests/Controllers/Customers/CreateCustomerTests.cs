using Demo.Application.Customers.Commands.CreateCustomer;
using Demo.Application.Customers.Events;
using Demo.Domain.Customer;
using Demo.Domain.Customer.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Demo.WebApi.Tests.Controllers.Customers.Helpers;
using Demo.WebApi.Tests.Helpers;
using FluentAssertions;
using FluentAssertions.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
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
        public async Task CreateCustomer_When_a_customer_can_be_created_It_should_return_statuscode_Created()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
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
        public async Task CreateCustomer_When_a_customer_is_created_It_should_be_persisted_to_the_database()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "Test"
            };

            // Act
            var response = await _client.CustomersController().CreateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();
            content.Id.Should().NotBeEmpty();

            var createdCustomerEntity = await FindExistingEntityAsync<Customer>(x => x.Id == content.Id);
            createdCustomerEntity.Should().NotBeNull();
            createdCustomerEntity.Name.Should().Be(command.Name);
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

            WithRetry(() => idFromEvent.Should().Be(content.Id), 1.Seconds(), 10.Milliseconds(), $"Didnt receive {nameof(ICustomerEventHub.CustomerCreated)} event for created entity");
        }

        [Fact]
        public async Task CreateCustomer_When_a_command_validator_throws_a_ValidationException_It_should_return_statuscode_BadRequest()
        {
            // Arrange
            //var client = _factory
            //    .WithWebHostBuilder(builder =>
            //    {
            //        builder.ConfigureTestServices(services =>
            //        {
            //            var mock = new Mock<IValidator<CreateCustomerCommand>>();
            //            mock.Setup(x => x.Validate(It.IsAny<CreateCustomerCommand>())).Throws(new ValidationException("'Name' must not be empty"));

            //            services.AddSingleton(mock.Object);
            //        });
            //    })
            //    .CreateClient();

            var command = new CreateCustomerCommand
            {
                Name = string.Empty
            };

            // Act
            var response = await _client.CustomersController().CreateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Type.Should().Be(nameof(ValidationException));
            content.Detail.Should().Contain("'Name' must not be empty");
        }

        [Fact]
        public async Task CreateCustomer_When_a_domainentity_validator_throws_a_DomainValidationException_It_should_return_statuscode_BadRequest()
        {
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var mock = new Mock<Domain.Shared.Interfaces.IValidator<Customer>>();
                        mock.Setup(x => x.ValidateAsync(It.IsAny<IDomainEntityContext<Customer>>(), It.IsAny<CancellationToken>()))
                            .Throws(new DomainValidationException(new[] { new ValidationMessage("TestProperty", "TestMessage") }));

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
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Type.Should().Be(nameof(DomainValidationException));
            content.Title.Should().Be("TestMessage");
        }

        [Fact]
        public async Task CreateCustomer_When_a_domainentity_hook_throws_a_DomainException_It_should_return_statuscode_BadRequest()
        {
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var mock = new Mock<IAfterCreate<Customer>>();
                        mock.Setup(x => x.ExecuteAsync(It.IsAny<HookType>(), It.IsAny<IDomainEntityContext<Customer>>(), It.IsAny<CancellationToken>()))
                            .Throws(new DomainException("TestMessage"));

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
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Type.Should().Be(nameof(DomainException));
            content.Title.Should().Be("TestMessage");
        }

        [Fact]
        public async Task CreateCustomer_When_an_unhandled_exception_occurs_It_should_return_statuscode_InternalServerError()
        {
            // Arrange
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var mock = new Mock<ICustomerDomainEntity>();
                        mock.Setup(x => x.CreateAsync(It.IsAny<CancellationToken>()))
                            .Throws(new ArgumentException("TestException"));

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
            content.Type.Should().Be(nameof(ArgumentException));
            content.Title.Should().Be("TestException");
        }

        [Fact]
        public async Task CreateCustomer_When_request_is_not_authenticated_It_should_return_statuscode_Unauthorized()
        {
            // Arrange
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var serviceDescriptor = services
                            .Where(x => x.ServiceType == typeof(IAuthorizationHandler))
                            .Where(x => x.ImplementationType == typeof(AllowUnauthorizedAuthorizationHandler))
                            .Single();
                        services.Remove(serviceDescriptor);
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
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
