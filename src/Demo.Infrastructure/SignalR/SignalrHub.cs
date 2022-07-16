using Demo.Application.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Demo.Infrastructure.SignalR;

public class SignalrHub : Hub<IEventHub>
{
}
