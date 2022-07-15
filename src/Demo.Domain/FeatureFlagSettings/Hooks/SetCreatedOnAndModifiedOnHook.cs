using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.FeatureFlagSettings.Hooks
{
    internal class SetCreatedOnAndModifiedOnHook : IBeforeCreate<FeatureFlagSettings>,
        IBeforeUpdate<FeatureFlagSettings>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IDateTime _dateTime;

        public SetCreatedOnAndModifiedOnHook(
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _dateTime = dateTime;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<FeatureFlagSettings> context,
            CancellationToken cancellationToken = default)
        {
            foreach (var featureFlag in context.Entity.Settings.FeatureFlags)
            {
                var pristineFeatureFlag =
                    context.Pristine.Settings.FeatureFlags.FirstOrDefault(x => x.Name == featureFlag.Name);

                if (pristineFeatureFlag == null)
                {
                    ((IAuditableEntity)featureFlag).SetCreatedByAndCreatedOn(_currentUserIdProvider.Id,
                        _dateTime.UtcNow);
                }
                else
                {
                    ((IAuditableEntity)featureFlag).SetCreatedByAndCreatedOn(pristineFeatureFlag.CreatedBy,
                        pristineFeatureFlag.CreatedOn);

                    if (
                        featureFlag.Description != pristineFeatureFlag.Description
                        || featureFlag.EnabledForAll != pristineFeatureFlag.EnabledForAll
                        || string.Join(",", featureFlag.EnabledForUsers) !=
                        string.Join(",", pristineFeatureFlag.EnabledForUsers)
                    )
                    {
                        ((IAuditableEntity)featureFlag).SetLastModifiedByAndLastModifiedOn(_currentUserIdProvider.Id,
                            _dateTime.UtcNow);
                    }
                    else
                    {
                        ((IAuditableEntity)featureFlag).SetLastModifiedByAndLastModifiedOn(
                            pristineFeatureFlag.LastModifiedBy, pristineFeatureFlag.LastModifiedOn!.Value);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
