using System;
using System.Threading.Tasks;

namespace Demo.Application.Roles.Events;

public interface IRoleEventHub
{
    Task RoleCreated(Guid id, Guid createdBy);
    Task RoleUpdated(Guid id, Guid updatedBy);
    Task RoleDeleted(Guid id, Guid deletedBy);

    // SCAFFOLD-MARKER: EVENTHUB
}
