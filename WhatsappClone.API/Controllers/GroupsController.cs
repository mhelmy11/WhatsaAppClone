using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Groups.Commands.Models;
using WhatsappClone.Core.Features.Groups.Queries.Models;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : AppControllerBase
    {




        [HttpPost("create-group")]

        [Authorize]
        public async Task<IActionResult> CreateGroup([FromForm] CreateGroupCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }


        [HttpPost("add-member-list")]
        [Authorize]
        public async Task<IActionResult> AddMembersList([FromForm] AddListOfMembersCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }


        [HttpGet("group-list")]
        [Authorize]
        public async Task<IActionResult> GetGroupList()
        {
            var result = await mediator.Send(new GetGroupListQuery());
            return ResponseResult(result);
        }

        [HttpDelete("remove-member")]
        [Authorize]


        public async Task<IActionResult> RemoveMember([FromQuery] RemoveMemberCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }


        [HttpDelete("leave-group")]
        [Authorize]


        public async Task<IActionResult> LeaveGroup([FromQuery] LeaveGroupCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }


        [HttpPut("update-group-picture")]
        [Authorize]
        public async Task<IActionResult> UpdateGroupPicture([FromForm] EditGroupPhotoCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

        [HttpPut("update-group-message")]
        [Authorize]
        public async Task<IActionResult> UpdateGroupMessage([FromForm] EditGroupMessageCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }
        [HttpDelete("delete-group-message")]
        [Authorize]
        public async Task<IActionResult> DeleteGroupMessage([FromForm] DeleteGroupMessageCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

    }
}
