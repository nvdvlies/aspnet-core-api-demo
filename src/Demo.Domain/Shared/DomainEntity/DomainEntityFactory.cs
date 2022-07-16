using System;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Domain.Shared.DomainEntity;

internal class DomainEntityFactory : IDomainEntityFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEntityFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T CreateInstance<T>() where T : IDomainEntity
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
