using Demo.Application.Shared.Interfaces;
using Demo.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;

namespace Demo.WebApi.SignalR
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
        public IEventHub CurrentUser => _hubContext.Clients.User(_currentUser.Id.ToString());
        public IEventHub User(Guid userId) => _hubContext.Clients.User(userId.ToString());
    }
}
