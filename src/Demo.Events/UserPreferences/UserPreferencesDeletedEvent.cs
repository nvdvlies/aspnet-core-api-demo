using System;

namespace Demo.Events.UserPreferences
{
    public class UserPreferencesDeletedEvent : Event<UserPreferencesDeletedEvent, UserPreferencesDeletedEventData>
    {
        public static UserPreferencesDeletedEvent Create(Guid correlationId, Guid id, Guid deletedBy)
        {
            var data = new UserPreferencesDeletedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                DeletedBy = deletedBy
            };
            return new UserPreferencesDeletedEvent
            {
                Topic = Topics.UserPreferences,
                Subject = $"UserPreferences/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class UserPreferencesDeletedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
    }
}