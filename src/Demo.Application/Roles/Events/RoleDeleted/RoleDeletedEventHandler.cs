using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Roles.Events.RoleDeleted
{
    public class RoleDeletedEventHandler : INotificationHandler<RoleDeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<RoleDeletedEventHandler> _logger;

        public RoleDeletedEventHandler(
            ILogger<RoleDeletedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(RoleDeletedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(RoleDeletedEvent)}");
            return _eventHubContext.All.RoleDeleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}
