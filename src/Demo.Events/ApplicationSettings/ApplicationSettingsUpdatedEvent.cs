using System;

namespace Demo.Events.ApplicationSettings
{
    public class ApplicationSettingsUpdatedEvent : Event<ApplicationSettingsUpdatedEvent, ApplicationSettingsUpdatedEventData>
    {
        public static ApplicationSettingsUpdatedEvent Create(string correlationId, Guid id, Guid updatedBy)
        {
            var data = new ApplicationSettingsUpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new ApplicationSettingsUpdatedEvent
            {
                Topic = Topics.ApplicationSettings,
                Subject = $"ApplicationSettings/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = data.CorrelationId
            };
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
