using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IQuery, IRequest<GetCurrentUserQueryResult>
    {
    }
}