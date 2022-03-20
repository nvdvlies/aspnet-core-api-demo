using MediatR;
using System;

namespace Demo.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IRequest<GetRoleByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}