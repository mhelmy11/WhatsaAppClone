using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsappClone.API.Base;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : AppControllerBase
    {



        //[HttpPost]
        //public async Task<IActionResult> Register([FromForm] AddUserCommand command)
        //{


        //    var result = await mediator.Send(command);

        //    return ResponseResult(result);
        //}


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
