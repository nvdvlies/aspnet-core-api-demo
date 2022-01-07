using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Events.Commands.ProcessIncomingEvents
{
    public class ProcessIncomingEventsCommandHandler : IRequestHandler<ProcessIncomingEventsCommand, Unit>
    {
        private readonly IMediator _mediator;

        public ProcessIncomingEventsCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProcessIncomingEventsCommand request, CancellationToken cancellationToken)
        {
            foreach (var @event in request.Events)
            {
                await _mediator.Publish(@event, cancellationToken);
            }

            return Unit.Value;
        }
    }
}