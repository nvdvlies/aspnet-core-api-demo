using System;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Events
{
    public interface ICustomerEventHub
    {
        Task CustomerCreated(Guid id, Guid createdBy);
        Task CustomerUpdated(Guid id, Guid updatedBy);
        Task CustomerDeleted(Guid id, Guid deletedBy);
    }
}