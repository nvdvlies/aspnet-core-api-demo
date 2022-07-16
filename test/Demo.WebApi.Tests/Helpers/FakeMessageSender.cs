using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.WebApi.Tests.Helpers;

public class FakeMessageSender : IMessageSender
{
    private readonly IServiceProvider _serviceProvider;

    public FakeMessageSender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(message, cancellationToken);
    }

    public async Task SendAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        foreach (var message in messages)
        {
            await mediator.Send(message, cancellationToken);
        }
    }
}
