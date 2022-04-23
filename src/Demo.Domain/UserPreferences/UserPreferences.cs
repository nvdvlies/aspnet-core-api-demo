using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.UserPreferences
{
    public partial class UserPreferences : AuditableEntity, IQueryableEntity
    {
        public UserPreferences()
        {
            Preferences = new UserPreferencesPreferences();
        }
        
        public User.User User { get; internal set; }
        
        [JsonInclude]
        public UserPreferencesPreferences Preferences { get; internal set; }
    }
}