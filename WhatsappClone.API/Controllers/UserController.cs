using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Users.Commands.BlockUser;
using WhatsappClone.Core.Features.Users.Commands.UnblockUser;
using WhatsappClone.Core.Features.Users.Commands.UpdatePrivacySettings;
using WhatsappClone.Core.Features.Users.Commands.UpdateProfile;
using WhatsappClone.Core.Features.Users.Queries;
using WhatsappClone.Core.Features.Users.Queries.GetBlockedUsers;
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


        [HttpPost("block-user")]
        public async Task<IActionResult> BlockUser([FromForm] BlockUserCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

        [HttpPost("unblock-user")]
        public async Task<IActionResult> UnblockUser([FromForm] UnblockUserCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

        [HttpPut("update-privacy-settings")]
        public async Task<IActionResult> UpdatePrivacySettings(UpdatePrivacySettingsCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }


        [HttpGet("get-blocked-users")]
        public async Task<IActionResult> GetBlockedUsers(GetBlockedUsersQuery query)
        {
            var result = await mediator.Send(query);
            return ResponseResult(result);
        }


        //[HttpPut("Me")]
        //[Authorize]
        //public async Task<IActionResult> EditMe([FromForm] EditMeCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

    }
}
