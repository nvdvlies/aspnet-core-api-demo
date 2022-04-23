using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.UserPreferences.Interfaces;

namespace Demo.Application.UserPreferences.Commands.SaveUserPreferences
{
    public class SaveUserPreferencesCommandHandler : IRequestHandler<SaveUserPreferencesCommand, Unit>
    {
        private readonly IUserPreferencesDomainEntity _userPreferencesDomainEntity;
        private readonly IMapper _mapper;

        public SaveUserPreferencesCommandHandler(
            IUserPreferencesDomainEntity userPreferencesDomainEntity,
            IMapper mapper
        )
        {
            _userPreferencesDomainEntity = userPreferencesDomainEntity;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(SaveUserPreferencesCommand request, CancellationToken cancellationToken)
        {
            await _userPreferencesDomainEntity.GetAsync(cancellationToken);

            _userPreferencesDomainEntity.MapFrom(request, _mapper);

            await _userPreferencesDomainEntity.UpsertAsync(cancellationToken);

            return Unit.Value;
        }
    }
}