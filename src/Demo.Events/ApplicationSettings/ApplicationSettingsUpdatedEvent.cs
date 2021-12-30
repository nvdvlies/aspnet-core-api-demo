using System;

namespace Demo.Events.ApplicationSettings
{
    public class ApplicationSettingsUpdatedEvent : Event<ApplicationSettingsUpdatedEventData>
    {
        public ApplicationSettingsUpdatedEvent(ApplicationSettingsUpdatedEventData data) : base(
            Topics.ApplicationSettings,
            data,
            $"ApplicationSettings/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static ApplicationSettingsUpdatedEvent Create(string correlationId, Guid id, Guid updatedBy)
        {
            var data = new ApplicationSettingsUpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new ApplicationSettingsUpdatedEvent(data);
        }
    }

    public class ApplicationSettingsUpdatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
