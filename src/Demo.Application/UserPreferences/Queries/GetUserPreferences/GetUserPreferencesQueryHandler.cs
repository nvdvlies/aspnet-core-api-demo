using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.UserPreferences.Queries.GetUserPreferences.Dtos;
using Demo.Domain.UserPreferences.Interfaces;
using MediatR;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferences
{
    public class
        GetUserPreferencesQueryHandler : IRequestHandler<GetUserPreferencesQuery, GetUserPreferencesQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IUserPreferencesProvider _userPreferencesProvider;

        public GetUserPreferencesQueryHandler(
            IUserPreferencesProvider userPreferencesProvider,
            IMapper mapper
        )
        {
            _userPreferencesProvider = userPreferencesProvider;
            _mapper = mapper;
        }

        public async Task<GetUserPreferencesQueryResult> Handle(GetUserPreferencesQuery request,
            CancellationToken cancellationToken)
        {
            var userPreferences = await _userPreferencesProvider.GetAsync(cancellationToken);

            var userPreferencesDto = _mapper.Map<UserPreferencesDto>(userPreferences);

            return new GetUserPreferencesQueryResult { UserPreferences = userPreferencesDto };
        }
    }
}