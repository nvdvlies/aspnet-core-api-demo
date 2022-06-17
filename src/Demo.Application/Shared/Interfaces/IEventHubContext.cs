using System;

namespace Demo.Application.Shared.Interfaces
{
    public interface IEventHubContext
    {
        IEventHub All { get; }
        IEventHub CurrentUser { get; }
        IEventHub User(string externalUserId);
    }
}
