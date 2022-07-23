using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Demo.WebApi.Tests.Helpers;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environmentName.Equals(
                "Development",
                StringComparison.OrdinalIgnoreCase))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "LocalIntegrationTest");
            builder.UseEnvironment("LocalIntegrationTest");
        }

        return base.CreateHost(builder);
    }
}
