using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using Demo.Domain.User.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    internal class UserProvider : IUserProvider
    {
        private const string CacheKeyPrefix = "Users";

        private readonly IDistributedCache _cache;
        private readonly IJsonService<User> _jsonService;
        private readonly IUserDomainEntity _userDomainEntity;

        public UserProvider(
            IDistributedCache cache,
            IJsonService<User> jsonService,
            IUserDomainEntity userDomainEntity
        )
        {
            _cache = cache;
            _jsonService = jsonService;
            _userDomainEntity = userDomainEntity;
        }

        public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetAsync(id, false, cancellationToken);
        }

        public async Task<User> GetAsync(Guid id, bool refreshCache, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CacheKeyPrefix}/{id}";
            var cacheValue = await _cache.GetAsync(cacheKey, cancellationToken);

            if (refreshCache || cacheValue == null)
            {
                await _userDomainEntity
                    .WithOptions(x => x.AsNoTracking = true)
                    .GetAsync(id, cancellationToken);

                var user = _userDomainEntity.Entity;

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                await _cache.SetAsync(cacheKey, Encode(user), cacheEntryOptions, cancellationToken);

                return user;
            }

            return Decode(cacheValue);
        }

        private User Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(User user)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(user));
        }
    }
}