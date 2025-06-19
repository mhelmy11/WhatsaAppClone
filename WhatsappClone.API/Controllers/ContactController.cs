using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Contacts.Commands.Models;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : AppControllerBase
    {
        [HttpPost("add-contact")]
        [Authorize]

        public async Task<IActionResult> AddContact([FromForm] AddContactCommand command)
        {

            var Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            command.userId = Id;
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }
    }
}
