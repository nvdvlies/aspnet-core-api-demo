using System.Text.Json.Serialization;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences
{
    public class UserPreferences : AuditableEntity, IQueryableEntity
    {
        public UserPreferences()
        {
            Preferences = new UserPreferencesPreferences();
        }

        public User.User User { get; internal set; }

        [JsonInclude] public UserPreferencesPreferences Preferences { get; internal set; }
    }
}