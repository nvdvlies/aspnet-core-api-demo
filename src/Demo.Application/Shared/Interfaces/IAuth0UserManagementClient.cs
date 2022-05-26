﻿using Demo.Domain.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces
{
    public interface IAuth0UserManagementClient
    {
        Task<string> CreateAsync(User internalUser, CancellationToken cancellationToken = default);
        Task<string> GetChangePasswordUrl(User internalUser, CancellationToken cancellationToken = default);
        Task SyncEmailToAuth0Async(User internalUser, CancellationToken cancellationToken = default);
        Task SyncNameToAuth0Async(User internalUser, CancellationToken cancellationToken = default);
        Task SyncRolesToAuth0Async(User internalUser, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
