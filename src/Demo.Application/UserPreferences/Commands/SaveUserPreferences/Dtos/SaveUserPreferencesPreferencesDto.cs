using System;

namespace Demo.Application.UserPreferences.Commands.SaveUserPreferences.Dtos
{
    public class SaveUserPreferencesPreferencesDto
    {
        public bool Setting1 { get; set; }
        public string Setting2 { get; set; }
        public DateTime Setting3 { get; set; }
        public Guid Setting4 { get; set; }
        public decimal Setting5 { get; set; }
    }
}