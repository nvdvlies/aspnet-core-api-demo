using Demo.Application.Shared.Dtos;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettings.Dtos
{
    public class ApplicationSettingsDto : AuditableEntityDto
    {
        public ApplicationSettingsSettingsDto Settings { get; set; }
    }
}