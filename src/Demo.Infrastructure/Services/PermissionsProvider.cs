using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    internal class PermissionsProvider : IPermissionsProvider
    {
        public const string CacheKey = "Permissions";

        private readonly IDistributedCache _cache;
        private readonly IJsonService<List<Permission>> _jsonService;
        private readonly IDbQuery<Permission> _query;

        public PermissionsProvider(
            IDistributedCache cache,
            IJsonService<List<Permission>> jsonService,
            IDbQuery<Permission> query
        )
        {
            _cache = cache;
            _jsonService = jsonService;
            _query = query;
        }

        public Task<List<Permission>> GetAsync(CancellationToken cancellationToken)
        {
            return GetAsync(false, cancellationToken);
        }

        public async Task<List<Permission>> GetAsync(bool refreshCache, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKey;
            var cacheValue = await _cache.GetAsync(cacheKey, cancellationToken);

            if (refreshCache || cacheValue == null)
            {
                var permissions = await _query
                    .AsQueryable()
                    .Include(role => role.RolePermissions)
                    .ToListAsync(cancellationToken);

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                await _cache.SetAsync(cacheKey, Encode(permissions), cacheEntryOptions, cancellationToken);

                return permissions;
            }

            return Decode(cacheValue);
        }

        private List<Permission> Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(List<Permission> permissions)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(permissions));
        }
    }
}
