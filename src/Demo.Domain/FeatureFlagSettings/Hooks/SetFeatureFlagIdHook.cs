using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.FeatureFlagSettings.Hooks
{
    internal class SetFeatureFlagIdHook : IBeforeCreate<FeatureFlagSettings>, IBeforeUpdate<FeatureFlagSettings>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<FeatureFlagSettings> context, CancellationToken cancellationToken = default)
        {
            foreach (var featureFlag in context.Entity.Settings.FeatureFlags.Where(x => x.Id == default))
            {
                featureFlag.Id = Guid.NewGuid();
            }
            return Task.CompletedTask;
        }
    }
}
