using System;

namespace Demo.Domain.Shared.Interfaces;

public interface ICurrentUserIdProvider
{
    Guid Id { get; }
    string ExternalId { get; }

    void SetUserId(Guid id);
    void SetUserId(string externalId);
}
