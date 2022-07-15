using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Permissions.Queries.GetAllPermissions
{
    public class GetAllPermissionsQuery : IQuery, IRequest<GetAllPermissionsQueryResult>
    {
    }
}