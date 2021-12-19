using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.ApplicationSettings.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommandHandler : IRequestHandler<SaveApplicationSettingsCommand, Unit>
    {
        private readonly IApplicationSettingsDomainEntity _domainEntity;
        private readonly IMapper _mapper;

        public SaveApplicationSettingsCommandHandler(
            IApplicationSettingsDomainEntity domainEntity,
            IMapper mapper
        )
        {
            _domainEntity = domainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveApplicationSettingsCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(cancellationToken);

            _domainEntity.MapFrom(request, _mapper);

            await _domainEntity.UpsertAsync(cancellationToken);

            return Unit.Value;
        }
    }
}