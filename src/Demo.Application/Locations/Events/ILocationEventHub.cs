using System;
using System.Threading.Tasks;

namespace Demo.Application.Locations.Events;

public interface ILocationEventHub
{
    Task LocationCreated(Guid id, Guid createdBy);
    Task LocationUpdated(Guid id, Guid updatedBy);

    Task LocationDeleted(Guid id, Guid deletedBy);
    // SCAFFOLD-MARKER: EVENTHUB
}
