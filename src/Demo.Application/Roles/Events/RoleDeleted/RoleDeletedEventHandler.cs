using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;

namespace Demo.Application.Roles.Events.RoleDeleted
{
    public class RoleDeletedEventHandler : INotificationHandler<RoleDeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public RoleDeletedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(RoleDeletedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.RoleDeleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}