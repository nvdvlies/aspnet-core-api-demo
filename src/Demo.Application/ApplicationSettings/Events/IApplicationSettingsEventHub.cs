using System;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Events
{
    public interface IApplicationSettingsEventHub
    {
        Task ApplicationSettingsUpdated(Guid id, Guid updatedBy);
    }
}
