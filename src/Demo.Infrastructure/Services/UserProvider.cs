using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using Demo.Domain.User.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Services
{
    internal class UserProvider : IUserProvider
    {
        private const string CacheKeyPrefix = "Users";

        private readonly IMemoryCache _memoryCache;
        private readonly IUserDomainEntity _userDomainEntity;

        public UserProvider(
            IMemoryCache memoryCache,
            IUserDomainEntity userDomainEntity
        )
        {
            _memoryCache = memoryCache;
            _userDomainEntity = userDomainEntity;
        }

        public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetAsync(id, refreshCache: false, cancellationToken);
        }

        public async Task<User> GetAsync(Guid id, bool refreshCache, CancellationToken cancellationToken = default)
        {
            if (refreshCache || !_memoryCache.TryGetValue($"{CacheKeyPrefix}/{id}", out User user))
            {
                _userDomainEntity.WithOptions(x => x.AsNoTracking = true);
                await _userDomainEntity.GetAsync(id, cancellationToken);
                user = _userDomainEntity.Entity;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _memoryCache.Set($"{CacheKeyPrefix}/{id}", user, cacheEntryOptions);
            }

            return user;
        }
    }
}
