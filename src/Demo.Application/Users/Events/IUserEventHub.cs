using System;
using System.Threading.Tasks;

namespace Demo.Application.Users.Events
{
    public interface IUserEventHub
    {
        Task UserCreated(Guid id, Guid createdBy);
        Task UserUpdated(Guid id, Guid updatedBy);

        Task UserDeleted(Guid id, Guid deletedBy);
        // SCAFFOLD-MARKER: EVENTHUB
    }
}