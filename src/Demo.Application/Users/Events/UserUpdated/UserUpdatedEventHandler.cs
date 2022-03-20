using Demo.Application.Shared.Interfaces;
using Demo.Events.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Events.UserUpdated
{
    public class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public UserUpdatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(UserUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.UserUpdated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}
