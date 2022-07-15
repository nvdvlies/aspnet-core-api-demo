using System;
using System.Threading.Tasks;

namespace Demo.Application.CurrentUser.Events
{
    public interface ICurrentUserEventHub
    {
        Task CurrentUserUpdated(Guid updatedBy);

        // SCAFFOLD-MARKER: EVENTHUB
    }
}
