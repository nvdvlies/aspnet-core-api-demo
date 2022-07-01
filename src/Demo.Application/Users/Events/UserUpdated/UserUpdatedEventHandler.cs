using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Users.Events.UserUpdated
{
    public class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly IExternalUserIdProvider _externalUserIdProvider;
        private readonly ILogger<UserUpdatedEventHandler> _logger;

        public UserUpdatedEventHandler(
            ILogger<UserUpdatedEventHandler> logger,
            IEventHubContext eventHubContext,
            IExternalUserIdProvider externalUserIdProvider
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
            _externalUserIdProvider = externalUserIdProvider;
        }

        public async Task Handle(UserUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(UserUpdatedEvent)}");
            await _eventHubContext.All.UserUpdated(@event.Data.Id, @event.Data.UpdatedBy);
            var externalUserId = _externalUserIdProvider.Get(@event.Data.Id);
            if (!string.IsNullOrEmpty(externalUserId))
            {
                await _eventHubContext.User(externalUserId).CurrentUserUpdated(@event.Data.UpdatedBy);
            }
        }
    }
}