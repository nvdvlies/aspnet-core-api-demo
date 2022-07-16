using System;
using Demo.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddTransient(typeof(Lazy<>), typeof(LazyInstance<>));
        return services;
    }
}
