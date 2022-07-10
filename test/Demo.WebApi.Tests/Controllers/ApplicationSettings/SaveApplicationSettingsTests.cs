using System.Net;
using System.Threading.Tasks;
using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings;
using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings.Dtos;
using Demo.WebApi.Tests.Controllers.ApplicationSettings.Helpers;
using Demo.WebApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Demo.WebApi.Tests.Controllers.ApplicationSettings
{
    [Collection(nameof(SharedFixture))]
    public class SaveApplicationSettingsTests : TestBase
    {
        public SaveApplicationSettingsTests(SharedFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task
            SaveApplicationSettings_When_application_settings_can_be_saved_It_should_return_statuscode_NoContent()
        {
            // Arrange
            await SetTestUserWithPermission(Domain.Role.Permissions.ApplicationSettingsWrite);
            var command = new SaveApplicationSettingsCommand
            {
                Settings = new SaveApplicationSettingsSettingsDto { Setting1 = true }
            };

            // Act
            var response = await Client.ApplicationSettingsController().Save(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task
            SaveApplicationSettings_When_user_does_not_have_write_permission_It_should_return_statuscode_Forbidden()
        {
            // Arrange
            await SetTestUserWithPermission(Domain.Role.Permissions.ApplicationSettingsRead);
            var command = new SaveApplicationSettingsCommand
            {
                Settings = new SaveApplicationSettingsSettingsDto { Setting1 = true }
            };

            // Act
            var response = await Client.ApplicationSettingsController().Save(command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}