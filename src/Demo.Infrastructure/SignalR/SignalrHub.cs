using Demo.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Demo.Infrastructure.SignalR;

[Authorize]
public class SignalrHub : Hub<IEventHub>
{
}
