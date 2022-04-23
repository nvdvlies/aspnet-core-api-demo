using System;
using System.Threading.Tasks;

namespace Demo.Application.UserPreferences.Events
{
    public interface IUserPreferencesEventHub
    {
        Task UserPreferencesCreated(Guid id, Guid createdBy);
        Task UserPreferencesUpdated(Guid id, Guid updatedBy);
        Task UserPreferencesDeleted(Guid id, Guid deletedBy);
        // SCAFFOLD-MARKER: EVENTHUB
    }
}