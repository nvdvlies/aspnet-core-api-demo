using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Queries.GetCustomerAuditlog;

public class GetCustomerAuditlogQuery : IQuery, IRequest<GetCustomerAuditlogQueryResult>
{
    internal Guid CustomerId { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public void SetCustomerId(Guid id)
    {
        CustomerId = id;
    }
}
