using System;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Services
{
    internal class CurrentUserIdProvider : ICurrentUserIdProvider
    {
        private readonly IExternalUserIdProvider _externalUserIdProvider;
        private readonly IUserIdProvider _userIdProvider;

        public CurrentUserIdProvider(
            IUserIdProvider userIdProvider,
            IExternalUserIdProvider externalUserIdProvider)
        {
            _userIdProvider = userIdProvider;
            _externalUserIdProvider = externalUserIdProvider;
        }

        public Guid Id { get; private set; }

        public string ExternalId { get; private set; }

        public void SetUserId(Guid id)
        {
            Id = id;
            ExternalId = _externalUserIdProvider.Get(id);
        }

        public void SetUserId(string externalId)
        {
            ExternalId = externalId;
            Id = _userIdProvider.Get(externalId);
        }
    }
}
