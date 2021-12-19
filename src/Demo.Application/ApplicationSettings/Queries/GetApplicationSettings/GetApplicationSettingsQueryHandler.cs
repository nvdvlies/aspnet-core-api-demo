using AutoMapper;
using Demo.Application.ApplicationSettings.Queries.GetApplicationSettings.Dtos;
using Demo.Domain.ApplicationSettings.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettings
{
    public class GetApplicationSettingsQueryHandler : IRequestHandler<GetApplicationSettingsQuery, GetApplicationSettingsQueryResult>
    {
        private readonly IApplicationSettingsProvider _applicationSettingsProvider;
        private readonly IMapper _mapper;

        public GetApplicationSettingsQueryHandler(
            IApplicationSettingsProvider applicationSettingsProvider,
            IMapper mapper
        )
        {
            _applicationSettingsProvider = applicationSettingsProvider;
            _mapper = mapper;
        }

        public async Task<GetApplicationSettingsQueryResult> Handle(GetApplicationSettingsQuery request, CancellationToken cancellationToken)
        {
            var applicationSettings = await _applicationSettingsProvider.GetAsync(cancellationToken);

            var applicationSettingsDto = _mapper.Map<ApplicationSettingsDto>(applicationSettings);

            return new GetApplicationSettingsQueryResult
            {
                ApplicationSettings = applicationSettingsDto
            };
        }
    }
}