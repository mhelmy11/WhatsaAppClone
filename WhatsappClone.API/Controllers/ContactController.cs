using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Contacts.Commands;
using WhatsappClone.Core.Features.Contacts.Queries;
using WhatsappClone.Core.Features.Contacts.Queries.GetMyContacts;
using WhatsappClone.Core.Features.Users.Queries;
using WhatsappClone.Core.Wrappers;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : AppControllerBase
    {
        [HttpPost("add-contact")]
        //[Authorize]
        public async Task<IActionResult> AddContact([FromQuery] AddContactCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }


        [HttpGet("check-contact-exists")]
        public async Task<IActionResult> CheckContactExists([FromQuery] CheckContactExistsQuery query)
        {
            var result = await mediator.Send(query);
            return ResponseResult(result);
        }



        [HttpGet("get-contacts")]
        public async Task<IActionResult> GetMyContacts(GetMyContactsQuery query)
        {
            var result = await mediator.Send(query);
            return ResponseResult(result);
        }


        [HttpDelete("delete-contact")]
        public async Task<IActionResult> DeleteContact(DeleteContactCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }
    }
}
