using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IUserIdProvider
    {
        Guid Get(string externalId);
        Guid Get(string externalId, bool refreshCache);
    }
}