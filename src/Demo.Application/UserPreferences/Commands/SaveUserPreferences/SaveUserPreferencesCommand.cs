using Demo.Application.Shared.Interfaces;
using MediatR;
using Demo.Application.UserPreferences.Commands.SaveUserPreferences.Dtos;

namespace Demo.Application.UserPreferences.Commands.SaveUserPreferences
{
    public class SaveUserPreferencesCommand : ICommand, IRequest<Unit>
    {
        public byte[] Timestamp { get; set; }
        public SaveUserPreferencesPreferencesDto Preferences { get; set; }
    }
}