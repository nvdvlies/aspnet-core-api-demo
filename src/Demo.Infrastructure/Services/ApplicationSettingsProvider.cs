using Demo.Domain.ApplicationSettings;
using Demo.Domain.ApplicationSettings.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Services
{
    public class ApplicationSettingsProvider : IApplicationSettingsProvider
    {
        private const string CacheKey = "ApplicationSettings";

        private readonly IMemoryCache _memoryCache;
        private readonly IApplicationSettingsDomainEntity _applicationSettingsDomainEntity;

        public ApplicationSettingsProvider(
            IMemoryCache memoryCache,
            IApplicationSettingsDomainEntity applicationSettingsDomainEntity
        )
        {
            _memoryCache = memoryCache;
            _applicationSettingsDomainEntity = applicationSettingsDomainEntity;
        }

        public async Task<ApplicationSettings> GetAsync(CancellationToken cancellationToken = default)
        {
            return await GetAsync(refreshCache: false, cancellationToken);
        }

        public async Task<ApplicationSettings> GetAsync(bool refreshCache, CancellationToken cancellationToken = default)
        {
            if (refreshCache || !_memoryCache.TryGetValue(CacheKey, out ApplicationSettings applicationSettings))
            {
                _applicationSettingsDomainEntity.WithOptions(x => x.AsNoTracking = true);
                await _applicationSettingsDomainEntity.GetAsync(cancellationToken);
                applicationSettings = _applicationSettingsDomainEntity.Entity;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _memoryCache.Set(CacheKey, applicationSettings, cacheEntryOptions);
            }

            return applicationSettings;
        }
    }
}
