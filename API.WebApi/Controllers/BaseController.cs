using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        //Для формирования команд при выполнении запросов
        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
