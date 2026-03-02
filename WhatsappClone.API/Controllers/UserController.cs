using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Users.Queries;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AppControllerBase
    {



        [HttpGet("check-user-exists")]
        public async Task<IActionResult> CheckUserExists([FromQuery] CheckUserExistsQuery command)
        {


            var result = await mediator.Send(command);

            return ResponseResult(result);
        }
        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetUserProfile([FromQuery] GetProfileQuery command)
        {
            var result = await mediator.Send(command);

            return ResponseResult(result);
        }


        //[HttpPost("forget-password")]
        //public async Task<IActionResult> ForgetPassword([FromForm] ForgetPasswordCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpGet("Me")]
        //[Authorize]
        //public async Task<IActionResult> GetMe()
        //{
        //    var result = await mediator.Send(new GetMeQuery());
        //    return ResponseResult(result);
        //}


        //[HttpPut("Me")]
        //[Authorize]
        //public async Task<IActionResult> EditMe([FromForm] EditMeCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

    }
}
