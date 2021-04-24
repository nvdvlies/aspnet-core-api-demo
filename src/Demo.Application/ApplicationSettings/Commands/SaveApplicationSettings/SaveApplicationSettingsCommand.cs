using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings.Dtos;
using MediatR;
using System;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommand : IRequest<Unit>
    {
        public byte[] Timestamp { get; set; }
        public SaveApplicationSettingsSettingsDto Settings { get; set; }
    }
}