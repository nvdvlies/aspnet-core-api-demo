using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.User;
using MediatR;

namespace Demo.Application.Users.Events.UserCreated
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public UserCreatedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.UserCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}