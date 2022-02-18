using System;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Events
{
    public interface ICustomerEventHub
    {
        Task CustomerCreated(Guid id, string createdBy);
        Task CustomerUpdated(Guid id, string updatedBy);
        Task CustomerDeleted(Guid id, string deletedBy);
    }
}
