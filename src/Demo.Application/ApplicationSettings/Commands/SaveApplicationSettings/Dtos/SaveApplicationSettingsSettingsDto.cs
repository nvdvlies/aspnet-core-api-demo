using System;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings.Dtos
{
    public class SaveApplicationSettingsSettingsDto
    {
        public bool Setting1 { get; set; }
        public string Setting2 { get; set; }
        public DateTime Setting3 { get; set; }
        public Guid Setting4 { get; set; }
        public decimal Setting5 { get; set; }
    }
}
