using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserPermissions
{
    public class GetCurrentUserPermissionsQuery : IQuery, IRequest<GetCurrentUserPermissionsQueryResult>
    {
    }
}