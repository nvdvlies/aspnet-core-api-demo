using Demo.Application.Shared.Dtos;

namespace Demo.Application.Customers.Queries.GetCustomerById.Dtos
{
    public class CustomerDto : SoftDeleteEntityDto
    {
        public string Name { get; set; }
    }
}