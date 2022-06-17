using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using IUserIdProvider = Microsoft.AspNetCore.SignalR.IUserIdProvider;

namespace Demo.Infrastructure.SignalR
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}