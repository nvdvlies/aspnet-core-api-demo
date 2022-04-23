using System;

namespace Demo.Events.UserPreferences
{
    public class UserPreferencesCreatedEvent : Event<UserPreferencesCreatedEvent, UserPreferencesCreatedEventData>
    {
        public static UserPreferencesCreatedEvent Create(string correlationId, Guid id, Guid createdBy)
        {
            var data = new UserPreferencesCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new UserPreferencesCreatedEvent
            {
                Topic = Topics.UserPreferences,
                Subject = $"UserPreferences/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class UserPreferencesCreatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}