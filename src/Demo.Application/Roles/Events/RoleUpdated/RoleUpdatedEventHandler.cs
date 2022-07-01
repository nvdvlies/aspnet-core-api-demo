using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Roles.Events.RoleUpdated
{
    public class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<RoleUpdatedEventHandler> _logger;

        public RoleUpdatedEventHandler(
            ILogger<RoleUpdatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(RoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(RoleUpdatedEvent)}");
            await _eventHubContext.All.RoleUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}