using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;

namespace Demo.Application.Roles.Events.RoleCreated
{
    public class RoleCreatedEventHandler : INotificationHandler<RoleCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public RoleCreatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(RoleCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.RoleCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}