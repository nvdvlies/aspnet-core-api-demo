using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserPermissions
{
    public class GetCurrentUserPermissionsQuery : IQuery, IRequest<GetCurrentUserPermissionsQueryResult>
    {
    }
}