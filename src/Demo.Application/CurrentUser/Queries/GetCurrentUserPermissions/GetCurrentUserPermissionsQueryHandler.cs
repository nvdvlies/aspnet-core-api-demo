using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Shared.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserPermissions
{
    public class
        GetCurrentUserPermissionsQueryHandler : IRequestHandler<GetCurrentUserPermissionsQuery,
            GetCurrentUserPermissionsQueryResult>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IMapper _mapper;
        private readonly IUserPermissionsProvider _userPermissionsProvider;

        public GetCurrentUserPermissionsQueryHandler(
            ICurrentUserIdProvider currentUserIdProvider,
            IUserPermissionsProvider userPermissionsProvider,
            IMapper mapper
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _userPermissionsProvider = userPermissionsProvider;
            _mapper = mapper;
        }

        public async Task<GetCurrentUserPermissionsQueryResult> Handle(GetCurrentUserPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            var userPermissions = await _userPermissionsProvider.GetAsync(_currentUserIdProvider.Id, cancellationToken);

            return new GetCurrentUserPermissionsQueryResult
            {
                Permissions = _mapper.Map<IEnumerable<PermissionDto>>(userPermissions)
            };
        }
    }
}
