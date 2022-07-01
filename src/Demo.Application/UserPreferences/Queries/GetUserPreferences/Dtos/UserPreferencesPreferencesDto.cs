using System;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferences.Dtos
{
    public class UserPreferencesPreferencesDto
    {
        public bool Setting1 { get; set; }
        public string Setting2 { get; set; }
        public DateTime Setting3 { get; set; }
        public Guid Setting4 { get; set; }
        public decimal Setting5 { get; set; }
    }
}