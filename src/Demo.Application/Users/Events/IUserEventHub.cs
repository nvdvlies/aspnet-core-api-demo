using System;
using System.Threading.Tasks;

namespace Demo.Application.Users.Events
{
    public interface IUserEventHub
    {
        Task UserCreated(Guid id, string createdBy);
        Task UserUpdated(Guid id, string updatedBy);
        Task UserDeleted(Guid id, string deletedBy);
        // SCAFFOLD-MARKER: EVENTHUB
    }
}