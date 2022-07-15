using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Users.Queries.IsEmailAvailable
{
    public class IsEmailAvailableQuery : IQuery, IRequest<IsEmailAvailableQueryResult>
    {
        public string Email { get; set; }
        public Guid? IgnoreId { get; set; }
    }
}
