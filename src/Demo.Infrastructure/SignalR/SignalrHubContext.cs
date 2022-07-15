using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Demo.Infrastructure.SignalR
{
    public class SignalrHubContext : IEventHubContext
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IHubContext<SignalrHub, IEventHub> _hubContext;

        public SignalrHubContext(
            IHubContext<SignalrHub, IEventHub> hubContext,
            ICurrentUserIdProvider currentUserIdProvider
        )
        {
            _hubContext = hubContext;
            _currentUserIdProvider = currentUserIdProvider;
        }

        public IEventHub All => _hubContext.Clients.All;
        public IEventHub CurrentUser => _hubContext.Clients.User(_currentUserIdProvider.ExternalId);

        public IEventHub User(string externalUserId)
        {
            return _hubContext.Clients.User(externalUserId);
        }
    }
}