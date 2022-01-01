using Demo.WebApi.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(nameof(Policies.User))]
    [EnableCors()]
    public abstract class ApiControllerBase : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator
        {
            get
            {
                if (_mediator == null)
                {
                    _mediator = HttpContext.RequestServices.GetService<IMediator>();
                }
                return _mediator;
            }
        }
    }
}
