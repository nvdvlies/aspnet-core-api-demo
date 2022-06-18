using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommand : ICommand, IRequest<Unit>
    {
        public byte[] Timestamp { get; set; }
        public SaveApplicationSettingsSettingsDto Settings { get; set; }
    }
}
