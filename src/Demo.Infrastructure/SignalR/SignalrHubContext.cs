using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Demo.Infrastructure.SignalR
{
    public class SignalrHubContext : IEventHubContext
    {
        private readonly ICurrentUser _currentUser;
        private readonly IHubContext<SignalrHub, IEventHub> _hubContext;

        public SignalrHubContext(
            IHubContext<SignalrHub, IEventHub> hubContext,
            ICurrentUser currentUser
        )
        {
            _hubContext = hubContext;
            _currentUser = currentUser;
        }

        public IEventHub All => _hubContext.Clients.All;
        public IEventHub CurrentUser => _hubContext.Clients.User(_currentUser.ExternalId);

        public IEventHub User(string externalId)
        {
            return _hubContext.Clients.User(externalId);
        }
    }
}