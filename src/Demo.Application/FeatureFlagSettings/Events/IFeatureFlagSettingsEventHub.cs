using System;
using System.Threading.Tasks;

namespace Demo.Application.FeatureFlagSettings.Events
{
    public interface IFeatureFlagSettingsEventHub
    {
        Task FeatureFlagSettingsUpdated(Guid id, Guid updatedBy);
        // SCAFFOLD-MARKER: EVENTHUB
    }
}
