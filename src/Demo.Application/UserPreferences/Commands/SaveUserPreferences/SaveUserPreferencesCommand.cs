using Demo.Application.Shared.Interfaces;
using Demo.Application.UserPreferences.Commands.SaveUserPreferences.Dtos;
using MediatR;

namespace Demo.Application.UserPreferences.Commands.SaveUserPreferences;

public class SaveUserPreferencesCommand : ICommand, IRequest<Unit>
{
    // ReSharper disable once InconsistentNaming
    public uint xmin { get; set; }
    public SaveUserPreferencesPreferencesDto Preferences { get; set; }
}
