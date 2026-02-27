using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsappClone.API.Base;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : AppControllerBase
    {




        //[HttpPost("create-group")]

        //[Authorize]
        //public async Task<IActionResult> CreateGroup([FromForm] CreateGroupCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
        //[HttpPost("generate-invite-group")]
        //[Authorize]
        //public async Task<IActionResult> GenerateInviteGroupLink([FromForm] CreateInviteLinkCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
        //[HttpPost("reset-invite-link")]
        //[Authorize]
        //public async Task<IActionResult> ResetInviteGroupLink([FromForm] CreateInviteLinkCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
        //[HttpPost("join-group-via-link")]
        //[Authorize]
        //public async Task<IActionResult> JoinGroupViaLink([FromForm] JoinGroupViaLinkCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpPost("add-member-list")]
        //[Authorize]
        //public async Task<IActionResult> AddMembersList([FromForm] AddListOfMembersCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpGet("group-invite-info/{inviteCode}")]
        //[Authorize]
        //public async Task<IActionResult> GetGroupInviteInfo(string inviteCode)
        //{
        //    var result = await mediator.Send(new GetGroupInviteInfoQuery(inviteCode));
        //    return ResponseResult(result);
        //}

        //[HttpGet("group-list")]
        //[Authorize]
        //public async Task<IActionResult> GetGroupList()
        //{
        //    var result = await mediator.Send(new GetGroupListQuery());
        //    return ResponseResult(result);
        //}

        //[HttpDelete("remove-member")]
        //[Authorize]


        //public async Task<IActionResult> RemoveMember([FromQuery] RemoveMemberCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpDelete("leave-group")]
        //[Authorize]


        //public async Task<IActionResult> LeaveGroup([FromQuery] LeaveGroupCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpPut("update-group-picture")]
        //[Authorize]
        //public async Task<IActionResult> UpdateGroupPicture([FromForm] EditGroupPhotoCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

        //[HttpPut("update-group-message")]
        //[Authorize]
        //public async Task<IActionResult> UpdateGroupMessage([FromForm] EditGroupMessageCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
        //[HttpDelete("delete-group-message")]
        //[Authorize]
        //public async Task<IActionResult> DeleteGroupMessage([FromForm] DeleteGroupMessageCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpPut("promote-user")]
        //[Authorize]
        //public async Task<IActionResult> PromoteUser([FromForm] PromoteToAdminCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpPut("revoke-user")]
        //[Authorize]
        //public async Task<IActionResult> RevokeUser([FromForm] RevokeAdminCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

        //[HttpPut("update-group-description")]
        //[Authorize]
        //public async Task<IActionResult> UpdateGroupDescription([FromForm] EditGroupDescriptionCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

        //[HttpPut("update-group-name")]
        //[Authorize]
        //public async Task<IActionResult> UpdateGroupName([FromForm] EditGroupNameCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
        //[HttpPut("update-group-settings")]
        //[Authorize]
        //public async Task<IActionResult> UpdateGroupSettings([FromForm] EditGroupPermissionsCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

        //[HttpPut("pin-group")]
        //[Authorize]
        //public async Task<IActionResult> PinGroup([FromForm] TogglePinGroupCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
        //[HttpPut("archive-group")]
        //[Authorize]
        //public async Task<IActionResult> ArchiveGroup([FromForm] ToggleArchiveGroupCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

    }
}
