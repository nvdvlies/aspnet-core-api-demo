using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.FeatureFlagSettings;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    internal class FeatureFlagSettingsProvider : IFeatureFlagSettingsProvider
    {
        public const string CacheKey = "FeatureFlagSettings";

        private readonly IDistributedCache _cache;
        private readonly IFeatureFlagSettingsDomainEntity _featureFlagSettingsDomainEntity;
        private readonly IJsonService<FeatureFlagSettings> _jsonService;

        public FeatureFlagSettingsProvider(
            IDistributedCache cache,
            IJsonService<FeatureFlagSettings> jsonService,
            IFeatureFlagSettingsDomainEntity featureFlagSettingsDomainEntity
        )
        {
            _cache = cache;
            _jsonService = jsonService;
            _featureFlagSettingsDomainEntity = featureFlagSettingsDomainEntity;
        }

        public Task<FeatureFlagSettings> GetAsync(CancellationToken cancellationToken = default)
        {
            return GetAsync(false, cancellationToken);
        }

        public async Task<FeatureFlagSettings> GetAsync(bool refreshCache,
            CancellationToken cancellationToken = default)
        {
            var cacheValue = await _cache.GetAsync(CacheKey, cancellationToken);

            if (refreshCache || cacheValue == null)
            {
                _featureFlagSettingsDomainEntity.WithOptions(x => x.AsNoTracking = true);
                await _featureFlagSettingsDomainEntity.GetAsync(cancellationToken);

                var featureFlagSettings = _featureFlagSettingsDomainEntity.Entity;

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                await _cache.SetAsync(CacheKey, Encode(featureFlagSettings), cacheEntryOptions, cancellationToken);

                return featureFlagSettings;
            }

            return Decode(cacheValue);
        }

        private FeatureFlagSettings Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(FeatureFlagSettings featureFlagSettings)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(featureFlagSettings));
        }
    }
}