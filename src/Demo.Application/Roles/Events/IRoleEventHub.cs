using System;
using System.Threading.Tasks;

namespace Demo.Application.Roles.Events
{
    public interface IRoleEventHub
    {
        Task RoleCreated(Guid id, string createdBy);
        Task RoleUpdated(Guid id, string updatedBy);
        Task RoleDeleted(Guid id, string deletedBy);
        // SCAFFOLD-MARKER: EVENTHUB
    }
}