using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserFeatureFlags
{
    public class GetCurrentUserFeatureFlagsQueryHandler : IRequestHandler<GetCurrentUserFeatureFlagsQuery,
        GetCurrentUserFeatureFlagsQueryResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IFeatureFlagSettingsProvider _featureFlagSettingsProvider;

        public GetCurrentUserFeatureFlagsQueryHandler(
            IFeatureFlagSettingsProvider featureFlagSettingsProvider,
            ICurrentUser currentUser
        )
        {
            _featureFlagSettingsProvider = featureFlagSettingsProvider;
            _currentUser = currentUser;
        }

        public async Task<GetCurrentUserFeatureFlagsQueryResult> Handle(GetCurrentUserFeatureFlagsQuery request,
            CancellationToken cancellationToken)
        {
            var featureFlagSettings = await _featureFlagSettingsProvider.GetAsync(cancellationToken);

            var userFeatureflags = featureFlagSettings.Settings.FeatureFlags
                .Where(x => x.EnabledForAll || x.EnabledForUsers.Contains(_currentUser.Id))
                .Select(x => x.Name)
                .ToList();

            return new GetCurrentUserFeatureFlagsQueryResult { FeatureFlags = userFeatureflags };
        }
    }
}
