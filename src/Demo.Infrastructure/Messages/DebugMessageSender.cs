using Demo.Application.Shared.Interfaces;
using Demo.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal class DebugMessageSender : IMessageSender
    {
        private readonly IServiceProvider _serviceProvider;

        public DebugMessageSender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var serviceBusMessage = message.ToServiceBusMessage();
                var message2 = serviceBusMessage.ToMessage();

                await mediator.Send(message2, cancellationToken);
            }
        }

        public async Task SendAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                foreach (var message in messages)
                {
                    var serviceBusMessage = message.ToServiceBusMessage();
                    var message2 = serviceBusMessage.ToMessage();

                    await mediator.Send(message2, cancellationToken);
                }
            }
        }
    }
}
