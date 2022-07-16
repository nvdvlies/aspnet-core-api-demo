using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Events;
using Demo.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Demo.Infrastructure.Messages.Consumers;

public abstract class BaseMessageConsumer<TMessage> where TMessage : class, IMessage
{
    private readonly IServiceProvider _serviceProvider;

    protected BaseMessageConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected ILogger<RabbitMqEventConsumer> Logger { get; private set; }
    protected IMediator Mediator { get; private set; }
    protected abstract Task ConsumeInternal(ConsumeContext<TMessage> context);

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            Logger = scope.ServiceProvider.GetRequiredService<ILogger<RabbitMqEventConsumer>>();
            var correlationIdProvider = scope.ServiceProvider.GetRequiredService<ICorrelationIdProvider>();
            var currentUserIdProvider = scope.ServiceProvider.GetRequiredService<ICurrentUserIdProvider>();
            Mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            currentUserIdProvider.SetUserId(context.Message.CreatedBy);
            correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
            using (LogContext.PushProperty("CorrelationId", correlationIdProvider.Id))
            {
                await ConsumeInternal(context);
            }
        }
    }
}
