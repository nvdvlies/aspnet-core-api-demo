using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Services
{
    internal class FeatureFlagChecker : IFeatureFlagChecker
    {
        private readonly IFeatureFlagSettingsProvider _featureFlagSettingsProvider;
        private readonly ICurrentUser _currentUser;

        public FeatureFlagChecker(
            IFeatureFlagSettingsProvider featureFlagSettingsProvider,
            ICurrentUser currentUser
        )
        {
            _featureFlagSettingsProvider = featureFlagSettingsProvider;
            _currentUser = currentUser;
        }

        public async Task<bool> IsEnabledAsync(string name, CancellationToken cancellationToken = default)
        {
            var featureFlagSettings = await _featureFlagSettingsProvider.GetAsync(cancellationToken);
            return featureFlagSettings.FeatureFlags.Any(x => x.Name == name && (x.EnabledForAll || x.EnabledForUsers.Contains(_currentUser.Id)));
        }
    }
}
