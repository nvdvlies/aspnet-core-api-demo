using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    internal class RolesProvider : IRolesProvider
    {
        public const string CacheKey = "Roles";

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

        public Task<List<Role>> GetAsync()
        {
            return GetAsync(false);
        }

        public async Task<List<Role>> GetAsync(bool refreshCache)
        {
            var cacheKey = CacheKey;
            var cacheValue = _cache.Get(cacheKey);

            if (refreshCache || cacheValue == null)
            {
                var roles = await _query
                    .AsQueryable()
                    .Include(role => role.Permissions)
                    .ThenInclude(rolePermission => rolePermission.Permission)
                    .ToListAsync();

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