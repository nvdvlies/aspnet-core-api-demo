using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Customers.Commands.UpdateCustomer;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Demo.WebApi.Tests.Controllers.Customers
{
    [Collection(nameof(SharedFixture))]
    public class UpdateCustomerTests : TestBase
    {
        public UpdateCustomerTests(SharedFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task UpdateCustomer_When_a_customer_is_updated_It_should_return_statuscode_Updated()
        {
            // Arrange
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2", xmin = existingCustomer.xmin };

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateCustomer_When_a_customer_is_updated_It_should_be_persisted_to_the_database()
        {
            // Arrange
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2", xmin = existingCustomer.xmin };

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updatedCustomerEntity = await FindExistingEntityAsync<Customer>(x => x.Id == customerId);
            updatedCustomerEntity.Should().NotBeNull();
            updatedCustomerEntity.Name.Should().Be(command.Name);
        }

        [Fact]
        public async Task UpdateCustomer_When_a_customer_is_updated_It_should_publish_a_CustomerUpdated_event()
        {
            // Arrange
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2", xmin = existingCustomer.xmin };

            var idFromEvent = Guid.Empty;
            using var subscription = HubConnection.On(nameof(ICustomerEventHub.CustomerUpdated),
                (Guid id, string _) => { idFromEvent = id; });

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            WithRetry(() => idFromEvent.Should().Be(customerId), 1.Seconds(), 10.Milliseconds(),
                $"Didnt receive {nameof(ICustomerEventHub.CustomerUpdated)} event for created entity");
        }

        [Fact]
        public async Task
            UpdateCustomer_When_a_command_validator_throws_a_ValidationException_It_should_return_statuscode_BadRequest()
        {
            // Arrange
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = string.Empty };

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content!.Type.Should().Be(nameof(ValidationException));
            content!.Detail.Should().Contain("'Name' must not be empty");
        }

        [Fact]
        public async Task
            UpdateCustomer_When_a_domainentity_validator_throws_a_DomainValidationException_It_should_return_statuscode_BadRequest()
        {
            // Arrange
            var client = CreateHttpClientWithCustomConfiguration(services =>
            {
                var mock = new Mock<Domain.Shared.Interfaces.IValidator<Customer>>();
                mock.Setup(x =>
                        x.ValidateAsync(It.IsAny<IDomainEntityContext<Customer>>(), It.IsAny<CancellationToken>()))
                    .Throws(new DomainValidationException(
                        new[] { new ValidationMessage("TestProperty", "TestMessage") }));

                services.AddSingleton(mock.Object);
            });
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);

            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2" };

            // Act
            var response = await client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content!.Type.Should().Be(nameof(DomainValidationException));
            content!.Title.Should().Be("TestMessage");
        }

        [Fact]
        public async Task
            UpdateCustomer_When_a_domainentity_hook_throws_a_DomainException_It_should_return_statuscode_BadRequest()
        {
            // Arrange
            var client = CreateHttpClientWithCustomConfiguration(services =>
            {
                var mock = new Mock<IAfterUpdate<Customer>>();
                mock.Setup(x => x.ExecuteAsync(It.IsAny<HookType>(), It.IsAny<IDomainEntityContext<Customer>>(),
                        It.IsAny<CancellationToken>()))
                    .Throws(new DomainException("TestMessage"));

                services.AddSingleton(mock.Object);
            });
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);

            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2" };

            // Act
            var response = await client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content!.Type.Should().Be(nameof(DomainException));
            content!.Title.Should().Be("TestMessage");
        }

        [Fact]
        public async Task
            UpdateCustomer_When_an_unhandled_exception_occurs_It_should_return_statuscode_InternalServerError()
        {
            // Arrange
            var client = CreateHttpClientWithCustomConfiguration(services =>
            {
                var mock = new Mock<ICustomerDomainEntity>();
                mock.Setup(x => x.UpdateAsync(It.IsAny<CancellationToken>()))
                    .Throws(new ArgumentException("TestException"));

                services.AddSingleton(mock.Object);
            });
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);

            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2" };

            // Act
            var response = await client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content!.Type.Should().Be(nameof(ArgumentException));
            content!.Title.Should().Be("TestException");
        }

        [Fact]
        public async Task UpdateCustomer_When_user_has_no_permission_It_should_return_statuscode_Forbidden()
        {
            // Arrange
            await SetTestUserWithoutPermissionAsync();
            var command = new UpdateCustomerCommand { Id = Guid.NewGuid(), Name = "Test" };

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateCustomer_When_request_is_not_authenticated_It_should_return_statuscode_Unauthorized()
        {
            // Arrange
            await SetTestUserToUnauthenticatedAsync();
            var command = new UpdateCustomerCommand { Id = Guid.NewGuid(), Name = "Test" };

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task
            UpdateCustomer_When_an_incorrect_concurrency_token_is_used_It_should_return_statuscode_BadRequest()
        {
            // Arrange
            await SetTestUserWithPermissionAsync(Domain.Role.Permissions.CustomersWrite);
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId, Name = "Test" };
            await AddAsExistingEntityAsync(existingCustomer);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Test2", xmin = 1 };

            // Act
            var response = await Client.CustomersController().UpdateAsync(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content!.Type.Should().Be(nameof(DbUpdateConcurrencyException));
        }
    }
}