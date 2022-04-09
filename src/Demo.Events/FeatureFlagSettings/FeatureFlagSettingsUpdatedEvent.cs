using System;

namespace Demo.Events.FeatureFlagSettings
{
    public class FeatureFlagSettingsUpdatedEvent : Event<FeatureFlagSettingsUpdatedEvent, FeatureFlagSettingsUpdatedEventData>
    {
        public static FeatureFlagSettingsUpdatedEvent Create(string correlationId, Guid id, Guid updatedBy)
        {
            var data = new FeatureFlagSettingsUpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new FeatureFlagSettingsUpdatedEvent
            {
                Topic = Topics.FeatureFlagSettings,
                Subject = $"FeatureFlagSettings/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class FeatureFlagSettingsUpdatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}