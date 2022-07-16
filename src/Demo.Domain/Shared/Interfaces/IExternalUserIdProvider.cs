using System;

namespace Demo.Domain.Shared.Interfaces;

public interface IExternalUserIdProvider
{
    string Get(Guid userId);
    string Get(Guid userId, bool refreshCache);
}
