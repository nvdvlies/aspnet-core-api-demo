using Demo.Application.Shared.Interfaces;
using Demo.Infrastructure.Messages;
using Demo.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class FakeMessageSender : IMessageSender
    {
        private readonly IServiceProvider _serviceProvider;

        public FakeMessageSender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var eventGridEvent = message.ToServiceBusMessage();
                var message2 = eventGridEvent.ToMessage();

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
                    var eventGridEvent = message.ToServiceBusMessage();
                    var message2 = eventGridEvent.ToMessage();

                    await mediator.Send(message2, cancellationToken);
                }
            }
        }
    }
}
