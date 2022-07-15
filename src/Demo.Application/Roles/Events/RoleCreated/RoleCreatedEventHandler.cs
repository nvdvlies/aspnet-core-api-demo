using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Roles.Events.RoleCreated
{
    public class RoleCreatedEventHandler : INotificationHandler<RoleCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<RoleCreatedEventHandler> _logger;

        public RoleCreatedEventHandler(
            ILogger<RoleCreatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(RoleCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(RoleCreatedEvent)}");
            return _eventHubContext.All.RoleCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}