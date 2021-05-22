using Demo.Application.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Demo.WebApi.SignalR
{
    public class SignalrHub : Hub<IEventHub>
    {
    }
}
