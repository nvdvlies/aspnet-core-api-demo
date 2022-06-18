using System;

namespace Demo.Events.UserPreferences
{
    public class UserPreferencesUpdatedEvent : Event<UserPreferencesUpdatedEvent, UserPreferencesUpdatedEventData>
    {
        public static UserPreferencesUpdatedEvent Create(Guid correlationId, Guid id, Guid updatedBy)
        {
            var data = new UserPreferencesUpdatedEventData
            {
                CorrelationId = correlationId, Id = id, UpdatedBy = updatedBy
            };
            return new UserPreferencesUpdatedEvent
            {
                Topic = Topics.UserPreferences,
                Subject = $"UserPreferences/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class UserPreferencesUpdatedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
