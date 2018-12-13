using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MakiseSharpServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : Controller
    {
        private IMediator mediator;

        protected IMediator Mediator => mediator ?? (mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}