using System;
using System.Linq;
using System.Text;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    internal class ExternalUserIdProvider : IExternalUserIdProvider
    {
        private const string CacheKeyPrefix = "ExternalUsersIds";

        private readonly IDistributedCache _cache;
        private readonly IDbQuery<User> _query;

        public ExternalUserIdProvider(
            IDistributedCache cache,
            IDbQuery<User> query
        )
        {
            _cache = cache;
            _query = query;
        }

        public string Get(Guid userId)
        {
            return Get(userId, false);
        }

        public string Get(Guid userId, bool refreshCache)
        {
            var cacheKey = $"{CacheKeyPrefix}/{userId}";
            var cacheValue = _cache.Get(cacheKey);

            if (refreshCache || cacheValue == null)
            {
                var externalUserId = _query
                    .AsQueryable()
                    .Where(x => x.Id == userId)
                    .Select(x => x.ExternalId)
                    .FirstOrDefault();

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, Encode(externalUserId), cacheEntryOptions);

                return externalUserId;
            }

            return Decode(cacheValue);
        }

        private string Decode(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        private byte[] Encode(string id)
        {
            return Encoding.UTF8.GetBytes(id);
        }
    }
}
