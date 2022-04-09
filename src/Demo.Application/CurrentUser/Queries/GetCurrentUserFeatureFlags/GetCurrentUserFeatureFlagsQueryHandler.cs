using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserFeatureFlags
{
    public class GetCurrentUserFeatureFlagsQueryHandler : IRequestHandler<GetCurrentUserFeatureFlagsQuery, GetCurrentUserFeatureFlagsQueryResult>
    {
        private readonly IFeatureFlagSettingsProvider _featureFlagSettingsProvider;
        private readonly ICurrentUser _currentUser;

        public GetCurrentUserFeatureFlagsQueryHandler(
            IFeatureFlagSettingsProvider featureFlagSettingsProvider,
            ICurrentUser currentUser
        )
        {
            _featureFlagSettingsProvider = featureFlagSettingsProvider;
            _currentUser = currentUser;
        }

        public async Task<GetCurrentUserFeatureFlagsQueryResult> Handle(GetCurrentUserFeatureFlagsQuery request, CancellationToken cancellationToken)
        {
            var featureFlagSettings = await _featureFlagSettingsProvider.GetAsync(cancellationToken);

            var userFeatureflags = featureFlagSettings.FeatureFlags
                    .Where(x => x.EnabledForAll || x.EnabledForUsers.Contains(_currentUser.Id))
                    .Select(x => x.Name)
                    .ToList();

            return new GetCurrentUserFeatureFlagsQueryResult
            {
                FeatureFlags = userFeatureflags
            };
        }
    }
}