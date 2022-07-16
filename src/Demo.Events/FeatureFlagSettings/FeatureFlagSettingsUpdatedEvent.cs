using System;

namespace Demo.Events.FeatureFlagSettings;

public class
    FeatureFlagSettingsUpdatedEvent : Event<FeatureFlagSettingsUpdatedEvent, FeatureFlagSettingsUpdatedEventData>
{
    public static FeatureFlagSettingsUpdatedEvent Create(Guid correlationId, Guid id, Guid updatedBy)
    {
        var data = new FeatureFlagSettingsUpdatedEventData
        {
            CorrelationId = correlationId, Id = id, UpdatedBy = updatedBy
        };
        return new FeatureFlagSettingsUpdatedEvent
        {
            Topic = Topics.FeatureFlagSettings,
            Subject = $"FeatureFlagSettings/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CreatedBy = updatedBy,
            CorrelationId = correlationId
        };
    }
}

public class FeatureFlagSettingsUpdatedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid UpdatedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
