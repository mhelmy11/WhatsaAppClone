using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Identity.Commands;


namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]







    public class AuthenticationController : AppControllerBase
    {

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromForm] LoginCommand command)
        //{
        //    var result = await mediator.Send(command);

        //    return ResponseResult(result);

        //}


        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        //{
        //    // Assuming UserContext.User is set with the current user context
        //    var result = await mediator.Send(command);

        //    return ResponseResult(result);
        //}


        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromQuery] RequestOtpCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] VerifyOtpCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }
    }
}
