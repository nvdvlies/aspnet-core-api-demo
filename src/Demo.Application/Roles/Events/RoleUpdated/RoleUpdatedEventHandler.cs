using Demo.Application.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Roles.Events.RoleUpdated
{
    public class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public RoleUpdatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(RoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.RoleUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
