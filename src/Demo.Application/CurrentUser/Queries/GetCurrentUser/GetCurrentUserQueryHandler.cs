using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Application.CurrentUser.Queries.GetCurrentUser.Dtos;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserQueryResult>
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IMapper _mapper;
    private readonly IDbQuery<User> _query;

    public GetCurrentUserQueryHandler(
        IDbQuery<User> query,
        IMapper mapper,
        ICurrentUserIdProvider currentUserIdProvider
    )
    {
        _query = query;
        _mapper = mapper;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<GetCurrentUserQueryResult> Handle(GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _query.AsQueryable()
            .Include(user => user.UserRoles)
            .ProjectTo<CurrentUserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == _currentUserIdProvider.Id, cancellationToken);

        return new GetCurrentUserQueryResult { CurrentUser = user };
    }
}
