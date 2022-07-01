using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    internal class RolesProvider : IRolesProvider
    {
        private const string CacheKey = "Roles";

        private readonly IDistributedCache _cache;
        private readonly IJsonService<List<Role>> _jsonService;
        private readonly IDbQuery<Role> _query;

        public RolesProvider(
            IDistributedCache cache,
            IJsonService<List<Role>> jsonService,
            IDbQuery<Role> query
        )
        {
            _cache = cache;
            _jsonService = jsonService;
            _query = query;
        }

        public List<Role> Get()
        {
            return Get(false);
        }

        public List<Role> Get(bool refreshCache)
        {
            var cacheKey = CacheKey;
            var cacheValue = _cache.Get(cacheKey);

            if (refreshCache || cacheValue == null)
            {
                var roles = _query
                    .AsQueryable()
                    .ToList();

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                _cache.Set(cacheKey, Encode(roles), cacheEntryOptions);

                return roles;
            }

            return Decode(cacheValue);
        }

        private List<Role> Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(List<Role> roles)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(roles));
        }
    }
}