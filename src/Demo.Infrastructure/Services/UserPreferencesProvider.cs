using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences;
using Demo.Domain.UserPreferences.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Demo.Infrastructure.Services
{
    public class UserPreferencesProvider : IUserPreferencesProvider
    {
        private const string CacheKeyPrefix = "UserPreferences";

        private readonly IDistributedCache _cache;
        private readonly ICurrentUser _currentUser;
        private readonly IJsonService<UserPreferences> _jsonService;
        private readonly IUserPreferencesDomainEntity _userPreferencesDomainEntity;

        public UserPreferencesProvider(
            IDistributedCache cache,
            ICurrentUser currentUser,
            IJsonService<UserPreferences> jsonService,
            IUserPreferencesDomainEntity userPreferencesDomainEntity
        )
        {
            _cache = cache;
            _currentUser = currentUser;
            _jsonService = jsonService;
            _userPreferencesDomainEntity = userPreferencesDomainEntity;
        }
        
        public async Task<UserPreferences> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(refreshCache: false, cancellationToken);
        }

        public async Task<UserPreferences> GetAsync(bool refreshCache, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var cacheKey = $"{CacheKeyPrefix}/{userId}";
            var cacheValue = await _cache.GetAsync(cacheKey, cancellationToken);
            
            if (refreshCache || cacheValue == null)
            {
                _userPreferencesDomainEntity.WithOptions(x => x.AsNoTracking = true);
                await _userPreferencesDomainEntity.GetAsync(cancellationToken);

                var userPreferences = _userPreferencesDomainEntity.Entity;

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));
                await _cache.SetAsync(cacheKey, Encode(userPreferences), cacheEntryOptions, cancellationToken);

                return userPreferences;
            }
            else
            {
                return Decode(cacheValue);
            }
        }
        
        private UserPreferences Decode(byte[] value)
        {
            return _jsonService.FromJson(Encoding.UTF8.GetString(value));
        }

        private byte[] Encode(UserPreferences applicationSettings)
        {
            return Encoding.UTF8.GetBytes(_jsonService.ToJson(applicationSettings));
        }
    }
}