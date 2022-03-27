using Demo.Domain.ApplicationSettings;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Services
{
    public class ApplicationSettingsProvider : IApplicationSettingsProvider
    {
        private const string CacheKey = "ApplicationSettings";

        private readonly IDistributedCache _cache;
        private readonly IJsonService<ApplicationSettings> _jsonService;
        private readonly IApplicationSettingsDomainEntity _applicationSettingsDomainEntity;

        public ApplicationSettingsProvider(
            IDistributedCache cache,
            IJsonService<ApplicationSettings> jsonService,
            IApplicationSettingsDomainEntity applicationSettingsDomainEntity
        )
        {
            _cache = cache;
            _jsonService = jsonService;
            _applicationSettingsDomainEntity = applicationSettingsDomainEntity;
        }

        public async Task<ApplicationSettings> GetAsync(CancellationToken cancellationToken = default)
        {
            return await GetAsync(refreshCache: false, cancellationToken);
        }

        public async Task<ApplicationSettings> GetAsync(bool refreshCache, CancellationToken cancellationToken = default)
        {
            var cacheValue = await _cache.GetAsync(CacheKey, cancellationToken);

            if (refreshCache || cacheValue == null)
            {
                _applicationSettingsDomainEntity.WithOptions(x => x.AsNoTracking = true);
                await _applicationSettingsDomainEntity.GetAsync(cancellationToken);

                var applicationSettings = _applicationSettingsDomainEntity.Entity;

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));
                await _cache.SetAsync(CacheKey, Encode(applicationSettings), cacheEntryOptions, cancellationToken);

                return applicationSettings;
            }
            else
            {
                return Decode(cacheValue);
            }
        }

        private ApplicationSettings Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(ApplicationSettings applicationSettings)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(applicationSettings));
        }
    }
}