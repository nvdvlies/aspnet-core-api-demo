using Demo.Application.Shared.Interfaces;
using Demo.Events.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Events.UserDeleted
{
    public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public UserDeletedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(UserDeletedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.UserDeleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}
