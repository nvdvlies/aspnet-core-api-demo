using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Text;

namespace Demo.Infrastructure.Services
{
    internal class UserIdProvider : IUserIdProvider
    {
        private const string CacheKeyPrefix = "UsersIds";

        private readonly IDistributedCache _cache;
        private readonly IDbQuery<User> _query;

        public UserIdProvider(
            IDistributedCache cache,
            IDbQuery<User> query
        )
        {
            _cache = cache;
            _query = query;
        }

        public Guid Get(string externalId)
        {
            return Get(externalId, refreshCache: false);
        }

        public Guid Get(string externalId, bool refreshCache)
        {
            var cacheKey = $"{CacheKeyPrefix}/{externalId}";
            var cacheValue = _cache.Get(cacheKey);

            if (refreshCache || cacheValue == null)
            {
                var userId = _query
                    .AsQueryable()
                    .Where(x => x.ExternalId == externalId)
                    .Select(x => x.Id)
                    .FirstOrDefault(); 
   
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, Encode(userId), cacheEntryOptions);

                return userId;
            }
            else
            {
                return Decode(cacheValue);
            }
        }

        private Guid Decode(byte[] value)
        {
            return Guid.Parse(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(Guid id)
        {
            return Encoding.UTF8.GetBytes(id.ToString());
        }
    }
}
