using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.API.Base
{
    public class AppControllerBase : ControllerBase
    {

        IMediator _mediator;

        protected IMediator mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;





        public ObjectResult ResponseResult<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.InternalServerError:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                case HttpStatusCode.Unauthorized:
                    return StatusCode(StatusCodes.Status401Unauthorized, response);
                case HttpStatusCode.UnprocessableEntity:
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, response);
                default:
                    return Ok(response);
            }

        }
    }
}
