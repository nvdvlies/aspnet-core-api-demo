using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Demo.Infrastructure.SignalR
{
    public class SignalrHubContext : IEventHubContext
    {
        private readonly IHubContext<SignalrHub, IEventHub> _hubContext;
        private readonly ICurrentUser _currentUser;

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
        public IEventHub User(string externalId) => _hubContext.Clients.User(externalId);
    }
}
