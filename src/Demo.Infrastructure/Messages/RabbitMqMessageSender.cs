using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Common.Extensions;
using Demo.Messages;
using MassTransit;
using IMessageData = Demo.Messages.IMessageData;

namespace Demo.Infrastructure.Messages
{
    internal class RabbitMqMessageSender : IMessageSender
    {
        private readonly IBus _bus;

        public RabbitMqMessageSender(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            var assembly = typeof(Message<IMessage, IMessageData>).Assembly;
            var messageType = assembly.GetType(message.Type);
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{message.Queue.ToString().PascalToKebabCase()}"));
            await endpoint.Send(message, messageType, context => context.CorrelationId = message.CorrelationId,
                cancellationToken);
        }

        public async Task SendAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken)
        {
            var assembly = typeof(Message<IMessage, IMessageData>).Assembly;
            foreach (var message in messages)
            {
                var messageType = assembly.GetType(message.Type);
                var endpoint =
                    await _bus.GetSendEndpoint(new Uri($"queue:{message.Queue.ToString().PascalToKebabCase()}"));
                await endpoint.Send(message, messageType, context => context.CorrelationId = message.CorrelationId,
                    cancellationToken);
            }
        }
    }
}