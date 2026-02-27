using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsappClone.API.Base;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : AppControllerBase
    {
        //[HttpPost("add-contact")]
        //[Authorize]

        //public async Task<IActionResult> AddContact([FromForm] AddContactCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpGet("get-contacts")]
        //[Authorize]
        //public async Task<IActionResult> GetContacts()
        //{
        //    var result = await mediator.Send(new GetContactsQuery());
        //    return ResponseResult(result);
        //}



        //[HttpPost("edit-contact")]
        //[Authorize]
        //public async Task<IActionResult> EditContact(EditContactCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}


        //[HttpPost("delete-contact")]
        //[Authorize]
        //public async Task<IActionResult> DeleteContact(DeleteContactCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}
    }
}
