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
    internal class PermissionGroupsProvider : IPermissionGroupsProvider
    {
        public const string CacheKey = "PermissionGroups";

        private readonly IDistributedCache _cache;
        private readonly IJsonService<List<PermissionGroup>> _jsonService;
        private readonly IDbQuery<PermissionGroup> _query;

        public PermissionGroupsProvider(
            IDistributedCache cache,
            IJsonService<List<PermissionGroup>> jsonService,
            IDbQuery<PermissionGroup> query
        )
        {
            _cache = cache;
            _jsonService = jsonService;
            _query = query;
        }

        public Task<List<PermissionGroup>> GetAsync(CancellationToken cancellationToken)
        {
            return GetAsync(false, cancellationToken);
        }

        public async Task<List<PermissionGroup>> GetAsync(bool refreshCache, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKey;
            var cacheValue = await _cache.GetAsync(cacheKey, cancellationToken);

            if (refreshCache || cacheValue == null)
            {
                var permissionGroups = await _query
                    .AsQueryable()
                    .ToListAsync(cancellationToken);

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                await _cache.SetAsync(cacheKey, Encode(permissionGroups), cacheEntryOptions, cancellationToken);

                return permissionGroups;
            }

            return Decode(cacheValue);
        }

        private List<PermissionGroup> Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(List<PermissionGroup> permissionGroups)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(permissionGroups));
        }
    }
}