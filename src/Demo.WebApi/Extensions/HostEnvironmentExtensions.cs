using System;
using Microsoft.Extensions.Hosting;

namespace Demo.WebApi.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsDockerDev(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null)
        {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.IsEnvironment("DockerDev");
    }

    public static bool IsLocalIntegrationTest(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null)
        {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.IsEnvironment("LocalIntegrationTest");
    }

}
